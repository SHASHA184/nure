package handlers

import (
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"

	"github.com/gorilla/mux"
	"github.com/stripe/stripe-go/v81"

	"car-shop/internal/database"
	"car-shop/internal/models"
	"car-shop/internal/payment"
)

type Handler struct {
	DB           *database.DB
	StripeClient *payment.StripeClient
}

func NewHandler(db *database.DB, stripeClient *payment.StripeClient) *Handler {
	return &Handler{
		DB:           db,
		StripeClient: stripeClient,
	}
}

// Homepage handler
func (h *Handler) Homepage(w http.ResponseWriter, r *http.Request) {
	http.ServeFile(w, r, "templates/index.html")
}

// GetCars returns all available cars
func (h *Handler) GetCars(w http.ResponseWriter, r *http.Request) {
	cars, err := h.DB.GetAllCars()
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(cars)
}

// CreateCheckoutSession creates a Stripe checkout session
func (h *Handler) CreateCheckoutSession(w http.ResponseWriter, r *http.Request) {
	var req struct {
		CarID int64 `json:"car_id"`
	}

	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, "Invalid request body", http.StatusBadRequest)
		return
	}

	car, err := h.DB.GetCarByID(req.CarID)
	if err != nil {
		http.Error(w, "Database error", http.StatusInternalServerError)
		return
	}

	if car == nil {
		http.Error(w, "Car not found", http.StatusNotFound)
		return
	}

	if car.Stock <= 0 {
		http.Error(w, "Car out of stock", http.StatusBadRequest)
		return
	}

	// Create checkout session
	amountInCents := int64(car.Price * 100) // Convert to cents
	successURL := fmt.Sprintf("http://%s/payment/success?session_id={CHECKOUT_SESSION_ID}", r.Host)
	cancelURL := fmt.Sprintf("http://%s/payment/cancel", r.Host)

	session, err := h.StripeClient.CreateCheckoutSession(
		fmt.Sprintf("%s %s (%d)", car.Brand, car.Model, car.Year),
		amountInCents,
		successURL,
		cancelURL,
	)
	if err != nil {
		log.Printf("Failed to create checkout session: %v", err)
		http.Error(w, "Failed to create checkout session", http.StatusInternalServerError)
		return
	}

	// Create order in database (without user_id)
	_, err = h.DB.CreateOrder(0, car.ID, car.Price, session.ID)
	if err != nil {
		log.Printf("Failed to create order: %v", err)
		http.Error(w, "Failed to create order", http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(map[string]string{
		"session_id":      session.ID,
		"publishable_key": h.StripeClient.PublishableKey,
	})
}

// PaymentSuccess handles successful payment
func (h *Handler) PaymentSuccess(w http.ResponseWriter, r *http.Request) {
	http.ServeFile(w, r, "templates/success.html")
}

// PaymentCancel handles cancelled payment
func (h *Handler) PaymentCancel(w http.ResponseWriter, r *http.Request) {
	http.ServeFile(w, r, "templates/cancel.html")
}

// StripeWebhook handles Stripe webhook events
func (h *Handler) StripeWebhook(w http.ResponseWriter, r *http.Request) {
	const MaxBodyBytes = int64(65536)
	r.Body = http.MaxBytesReader(w, r.Body, MaxBodyBytes)
	payload, err := io.ReadAll(r.Body)
	if err != nil {
		log.Printf("Error reading request body: %v", err)
		w.WriteHeader(http.StatusServiceUnavailable)
		return
	}

	signature := r.Header.Get("Stripe-Signature")
	event, err := h.StripeClient.VerifyWebhookSignature(payload, signature)
	if err != nil {
		log.Printf("Webhook signature verification failed: %v", err)
		w.WriteHeader(http.StatusBadRequest)
		return
	}

	// Handle the event
	switch event.Type {
	case "checkout.session.completed":
		var session stripe.CheckoutSession
		err := json.Unmarshal(event.Data.Raw, &session)
		if err != nil {
			log.Printf("Error parsing webhook JSON: %v", err)
			w.WriteHeader(http.StatusBadRequest)
			return
		}

		// Get order by session ID
		order, err := h.DB.GetOrderBySessionID(session.ID)
		if err != nil {
			log.Printf("Failed to get order: %v", err)
			w.WriteHeader(http.StatusInternalServerError)
			return
		}

		if order == nil {
			log.Printf("Order not found for session: %s", session.ID)
			w.WriteHeader(http.StatusNotFound)
			return
		}

		// Update order status to paid
		err = h.DB.UpdateOrderStatus(order.ID, models.OrderStatusPaid, session.PaymentIntent.ID)
		if err != nil {
			log.Printf("Failed to update order status: %v", err)
			w.WriteHeader(http.StatusInternalServerError)
			return
		}

		// Decrease car stock
		car, err := h.DB.GetCarByID(order.CarID)
		if err != nil {
			log.Printf("Failed to get car: %v", err)
			w.WriteHeader(http.StatusInternalServerError)
			return
		}

		if car.Stock > 0 {
			err = h.DB.UpdateCarStock(car.ID, car.Stock-1)
			if err != nil {
				log.Printf("Failed to update car stock: %v", err)
				w.WriteHeader(http.StatusInternalServerError)
				return
			}
		}

		log.Printf("✓ Payment successful for order %d, car stock updated: %s %s (now %d in stock)",
			order.ID, car.Brand, car.Model, car.Stock-1)

	case "checkout.session.expired":
		var session stripe.CheckoutSession
		err := json.Unmarshal(event.Data.Raw, &session)
		if err != nil {
			log.Printf("Error parsing webhook JSON: %v", err)
			w.WriteHeader(http.StatusBadRequest)
			return
		}

		// Get order by session ID
		order, err := h.DB.GetOrderBySessionID(session.ID)
		if err != nil {
			log.Printf("Failed to get order: %v", err)
			w.WriteHeader(http.StatusInternalServerError)
			return
		}

		if order != nil {
			// Update order status to cancelled
			err = h.DB.UpdateOrderStatus(order.ID, models.OrderStatusCancelled, "")
			if err != nil {
				log.Printf("Failed to update order status: %v", err)
			}
		}

		log.Printf("✗ Payment session expired for order %d (stock unchanged)", order.ID)

	default:
		log.Printf("Unhandled event type: %s", event.Type)
	}

	w.WriteHeader(http.StatusOK)
}

// GetOrderStatus returns order status by session ID
func (h *Handler) GetOrderStatus(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	sessionID := vars["session_id"]

	order, err := h.DB.GetOrderBySessionID(sessionID)
	if err != nil {
		http.Error(w, "Database error", http.StatusInternalServerError)
		return
	}

	if order == nil {
		http.Error(w, "Order not found", http.StatusNotFound)
		return
	}

	// Get car info
	car, err := h.DB.GetCarByID(order.CarID)
	if err == nil && car != nil {
		order.Car = car
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(order)
}
