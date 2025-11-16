package main

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"

	"car-shop/internal/database"
	"car-shop/internal/handlers"
	"car-shop/internal/payment"
)

func main() {
	// Load environment variables
	if err := godotenv.Load(); err != nil {
		log.Println("No .env file found, using system environment variables")
	}

	// Initialize database
	dbPath := os.Getenv("DB_PATH")
	if dbPath == "" {
		dbPath = "./car-shop.db"
	}

	db, err := database.New(dbPath)
	if err != nil {
		log.Fatalf("Failed to initialize database: %v", err)
	}
	defer db.Close()

	// Initialize Stripe client
	stripeClient := payment.NewStripeClient()

	// Initialize handlers
	h := handlers.NewHandler(db, stripeClient)

	// Setup router
	r := mux.NewRouter()

	// Static files
	r.PathPrefix("/static/").Handler(http.StripPrefix("/static/", http.FileServer(http.Dir("static"))))

	// Pages
	r.HandleFunc("/", h.Homepage).Methods("GET")
	r.HandleFunc("/payment/success", h.PaymentSuccess).Methods("GET")
	r.HandleFunc("/payment/cancel", h.PaymentCancel).Methods("GET")

	// API routes
	api := r.PathPrefix("/api").Subrouter()
	api.HandleFunc("/cars", h.GetCars).Methods("GET")
	api.HandleFunc("/checkout", h.CreateCheckoutSession).Methods("POST")
	api.HandleFunc("/order/{session_id}", h.GetOrderStatus).Methods("GET")

	// Webhook route
	r.HandleFunc("/webhook/stripe", h.StripeWebhook).Methods("POST")

	// Start server
	port := os.Getenv("PORT")
	if port == "" {
		port = "8080"
	}

	host := os.Getenv("HOST")
	if host == "" {
		host = "localhost"
	}

	addr := fmt.Sprintf("%s:%s", host, port)
	log.Printf("Server starting on http://%s", addr)
	log.Printf("Make sure to configure Stripe keys in .env file")
	log.Fatal(http.ListenAndServe(addr, r))
}
