package models

import "time"

// User represents an authenticated user
type User struct {
	ID        int64     `json:"id"`
	Email     string    `json:"email"`
	Name      string    `json:"name"`
	Provider  string    `json:"provider"` // "google" or "github"
	CreatedAt time.Time `json:"created_at"`
}

// Car represents a car available for purchase
type Car struct {
	ID          int64   `json:"id"`
	Model       string  `json:"model"`
	Brand       string  `json:"brand"`
	Year        int     `json:"year"`
	Price       float64 `json:"price"`
	Stock       int     `json:"stock"`
	Description string  `json:"description"`
	ImageURL    string  `json:"image_url"`
}

// OrderStatus represents the status of an order
type OrderStatus string

const (
	OrderStatusPending   OrderStatus = "pending"
	OrderStatusPaid      OrderStatus = "paid"
	OrderStatusFailed    OrderStatus = "failed"
	OrderStatusCancelled OrderStatus = "cancelled"
)

// Order represents a customer's order
type Order struct {
	ID                int64       `json:"id"`
	UserID            int64       `json:"user_id"`
	CarID             int64       `json:"car_id"`
	Status            OrderStatus `json:"status"`
	TotalAmount       float64     `json:"total_amount"`
	StripePaymentID   string      `json:"stripe_payment_id,omitempty"`
	StripeSessionID   string      `json:"stripe_session_id,omitempty"`
	CreatedAt         time.Time   `json:"created_at"`
	UpdatedAt         time.Time   `json:"updated_at"`

	// Joined fields
	Car  *Car  `json:"car,omitempty"`
	User *User `json:"user,omitempty"`
}
