package database

import (
	"database/sql"
	"fmt"
	"time"

	_ "github.com/mattn/go-sqlite3"

	"car-shop/internal/models"
)

type DB struct {
	conn *sql.DB
}

// New creates a new database connection
func New(dbPath string) (*DB, error) {
	conn, err := sql.Open("sqlite3", dbPath)
	if err != nil {
		return nil, fmt.Errorf("failed to open database: %w", err)
	}

	if err := conn.Ping(); err != nil {
		return nil, fmt.Errorf("failed to ping database: %w", err)
	}

	db := &DB{conn: conn}

	if err := db.createTables(); err != nil {
		return nil, fmt.Errorf("failed to create tables: %w", err)
	}

	if err := db.seedData(); err != nil {
		return nil, fmt.Errorf("failed to seed data: %w", err)
	}

	return db, nil
}

// Close closes the database connection
func (db *DB) Close() error {
	return db.conn.Close()
}

// createTables creates all necessary tables
func (db *DB) createTables() error {
	queries := []string{
		`CREATE TABLE IF NOT EXISTS users (
			id INTEGER PRIMARY KEY AUTOINCREMENT,
			email TEXT NOT NULL UNIQUE,
			name TEXT NOT NULL,
			provider TEXT NOT NULL,
			created_at DATETIME DEFAULT CURRENT_TIMESTAMP
		)`,
		`CREATE TABLE IF NOT EXISTS cars (
			id INTEGER PRIMARY KEY AUTOINCREMENT,
			model TEXT NOT NULL,
			brand TEXT NOT NULL,
			year INTEGER NOT NULL,
			price REAL NOT NULL,
			stock INTEGER NOT NULL DEFAULT 0,
			description TEXT,
			image_url TEXT
		)`,
		`CREATE TABLE IF NOT EXISTS orders (
			id INTEGER PRIMARY KEY AUTOINCREMENT,
			user_id INTEGER NOT NULL,
			car_id INTEGER NOT NULL,
			status TEXT NOT NULL DEFAULT 'pending',
			total_amount REAL NOT NULL,
			stripe_payment_id TEXT,
			stripe_session_id TEXT,
			created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
			updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
			FOREIGN KEY (user_id) REFERENCES users(id),
			FOREIGN KEY (car_id) REFERENCES cars(id)
		)`,
	}

	for _, query := range queries {
		if _, err := db.conn.Exec(query); err != nil {
			return fmt.Errorf("failed to execute query: %w", err)
		}
	}

	return nil
}

// seedData seeds initial data if tables are empty
func (db *DB) seedData() error {
	var count int
	err := db.conn.QueryRow("SELECT COUNT(*) FROM cars").Scan(&count)
	if err != nil {
		return err
	}

	if count > 0 {
		return nil // Data already exists
	}

	cars := []struct {
		model       string
		brand       string
		year        int
		price       float64
		stock       int
		description string
		imageURL    string
	}{
		{
			"968", "ЗАЗ", 1972, 5000.00, 3,
			"Легендарний радянський автомобіль. Надійний та економічний.",
			"https://images.unsplash.com/photo-1552519507-da3b142c6e3d?w=800&q=80",
		},
		{
			"Lanos", "Chevrolet", 2008, 3500.00, 5,
			"Популярний компактний автомобіль. Відмінний варіант для міста.",
			"https://images.unsplash.com/photo-1583121274602-3e2820c69888?w=800&q=80",
		},
		{
			"Tavria", "ЗАЗ", 1995, 2000.00, 2,
			"Компактний міський автомобіль. Низька ціна обслуговування.",
			"https://images.unsplash.com/photo-1542362567-b07e54358753?w=800&q=80",
		},
	}

	for _, car := range cars {
		_, err := db.conn.Exec(
			`INSERT INTO cars (model, brand, year, price, stock, description, image_url)
			 VALUES (?, ?, ?, ?, ?, ?, ?)`,
			car.model, car.brand, car.year, car.price, car.stock, car.description, car.imageURL,
		)
		if err != nil {
			return fmt.Errorf("failed to insert car: %w", err)
		}
	}

	return nil
}

// User operations

func (db *DB) CreateUser(email, name, provider string) (*models.User, error) {
	result, err := db.conn.Exec(
		"INSERT INTO users (email, name, provider) VALUES (?, ?, ?)",
		email, name, provider,
	)
	if err != nil {
		return nil, err
	}

	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}

	return db.GetUserByID(id)
}

func (db *DB) GetUserByEmail(email string) (*models.User, error) {
	user := &models.User{}
	err := db.conn.QueryRow(
		"SELECT id, email, name, provider, created_at FROM users WHERE email = ?",
		email,
	).Scan(&user.ID, &user.Email, &user.Name, &user.Provider, &user.CreatedAt)

	if err == sql.ErrNoRows {
		return nil, nil
	}
	if err != nil {
		return nil, err
	}

	return user, nil
}

func (db *DB) GetUserByID(id int64) (*models.User, error) {
	user := &models.User{}
	err := db.conn.QueryRow(
		"SELECT id, email, name, provider, created_at FROM users WHERE id = ?",
		id,
	).Scan(&user.ID, &user.Email, &user.Name, &user.Provider, &user.CreatedAt)

	if err == sql.ErrNoRows {
		return nil, nil
	}
	if err != nil {
		return nil, err
	}

	return user, nil
}

// Car operations

func (db *DB) GetAllCars() ([]models.Car, error) {
	rows, err := db.conn.Query(
		"SELECT id, model, brand, year, price, stock, description, image_url FROM cars",
	)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	var cars []models.Car
	for rows.Next() {
		var car models.Car
		err := rows.Scan(
			&car.ID, &car.Model, &car.Brand, &car.Year,
			&car.Price, &car.Stock, &car.Description, &car.ImageURL,
		)
		if err != nil {
			return nil, err
		}
		cars = append(cars, car)
	}

	return cars, nil
}

func (db *DB) GetCarByID(id int64) (*models.Car, error) {
	car := &models.Car{}
	err := db.conn.QueryRow(
		"SELECT id, model, brand, year, price, stock, description, image_url FROM cars WHERE id = ?",
		id,
	).Scan(&car.ID, &car.Model, &car.Brand, &car.Year, &car.Price, &car.Stock, &car.Description, &car.ImageURL)

	if err == sql.ErrNoRows {
		return nil, nil
	}
	if err != nil {
		return nil, err
	}

	return car, nil
}

func (db *DB) UpdateCarStock(carID int64, newStock int) error {
	_, err := db.conn.Exec("UPDATE cars SET stock = ? WHERE id = ?", newStock, carID)
	return err
}

// Order operations

func (db *DB) CreateOrder(userID, carID int64, totalAmount float64, stripeSessionID string) (*models.Order, error) {
	result, err := db.conn.Exec(
		`INSERT INTO orders (user_id, car_id, status, total_amount, stripe_session_id)
		 VALUES (?, ?, ?, ?, ?)`,
		userID, carID, models.OrderStatusPending, totalAmount, stripeSessionID,
	)
	if err != nil {
		return nil, err
	}

	id, err := result.LastInsertId()
	if err != nil {
		return nil, err
	}

	return db.GetOrderByID(id)
}

func (db *DB) GetOrderByID(id int64) (*models.Order, error) {
	order := &models.Order{}
	err := db.conn.QueryRow(
		`SELECT id, user_id, car_id, status, total_amount,
		 COALESCE(stripe_payment_id, ''), COALESCE(stripe_session_id, ''),
		 created_at, updated_at
		 FROM orders WHERE id = ?`,
		id,
	).Scan(
		&order.ID, &order.UserID, &order.CarID, &order.Status,
		&order.TotalAmount, &order.StripePaymentID, &order.StripeSessionID,
		&order.CreatedAt, &order.UpdatedAt,
	)

	if err == sql.ErrNoRows {
		return nil, nil
	}
	if err != nil {
		return nil, err
	}

	return order, nil
}

func (db *DB) GetOrderBySessionID(sessionID string) (*models.Order, error) {
	order := &models.Order{}
	err := db.conn.QueryRow(
		`SELECT id, user_id, car_id, status, total_amount,
		 COALESCE(stripe_payment_id, ''), COALESCE(stripe_session_id, ''),
		 created_at, updated_at
		 FROM orders WHERE stripe_session_id = ?`,
		sessionID,
	).Scan(
		&order.ID, &order.UserID, &order.CarID, &order.Status,
		&order.TotalAmount, &order.StripePaymentID, &order.StripeSessionID,
		&order.CreatedAt, &order.UpdatedAt,
	)

	if err == sql.ErrNoRows {
		return nil, nil
	}
	if err != nil {
		return nil, err
	}

	return order, nil
}

func (db *DB) UpdateOrderStatus(orderID int64, status models.OrderStatus, paymentID string) error {
	_, err := db.conn.Exec(
		`UPDATE orders
		 SET status = ?, stripe_payment_id = ?, updated_at = ?
		 WHERE id = ?`,
		status, paymentID, time.Now(), orderID,
	)
	return err
}

func (db *DB) GetUserOrders(userID int64) ([]models.Order, error) {
	rows, err := db.conn.Query(
		`SELECT o.id, o.user_id, o.car_id, o.status, o.total_amount,
		 COALESCE(o.stripe_payment_id, ''), COALESCE(o.stripe_session_id, ''),
		 o.created_at, o.updated_at,
		 c.id, c.model, c.brand, c.year, c.price, c.stock, c.description, c.image_url
		 FROM orders o
		 JOIN cars c ON o.car_id = c.id
		 WHERE o.user_id = ?
		 ORDER BY o.created_at DESC`,
		userID,
	)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	var orders []models.Order
	for rows.Next() {
		var order models.Order
		var car models.Car
		err := rows.Scan(
			&order.ID, &order.UserID, &order.CarID, &order.Status,
			&order.TotalAmount, &order.StripePaymentID, &order.StripeSessionID,
			&order.CreatedAt, &order.UpdatedAt,
			&car.ID, &car.Model, &car.Brand, &car.Year,
			&car.Price, &car.Stock, &car.Description, &car.ImageURL,
		)
		if err != nil {
			return nil, err
		}
		order.Car = &car
		orders = append(orders, order)
	}

	return orders, nil
}
