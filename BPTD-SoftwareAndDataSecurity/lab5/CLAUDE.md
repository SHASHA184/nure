# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Car shop web application with Stripe payment integration (test mode). Built in Go with SQLite database.

## Build and Run Commands

```bash
# Install dependencies
go mod tidy

# Run the server (requires .env with Stripe keys)
go run cmd/server/main.go

# Build executable
go build -o car-shop cmd/server/main.go
```

## Stripe Webhook Testing

For local development, use Stripe CLI to forward webhooks:
```bash
stripe listen --forward-to localhost:8080/webhook/stripe
```

## Architecture

The application follows a layered architecture:

- **cmd/server/main.go** - Entry point. Initializes DB, Stripe client, handlers, and routes.
- **internal/models/** - Data models (Car, Order, User, OrderStatus)
- **internal/database/** - SQLite database layer with CRUD operations. Auto-creates tables and seeds initial car data on startup.
- **internal/payment/** - Stripe SDK wrapper for checkout sessions and webhook signature verification.
- **internal/handlers/** - HTTP handlers using gorilla/mux. Handles checkout flow and Stripe webhooks.
- **templates/** - HTML templates served directly (index.html, success.html, cancel.html)

## Key Patterns

- Orders are created with `pending` status when checkout session starts
- Stock decrements only after successful payment via webhook (`checkout.session.completed`)
- Cancelled/expired sessions update order to `cancelled` without stock change
- Webhook signature verification is mandatory for all Stripe events

## Environment Variables

Required in `.env`:
- `STRIPE_SECRET_KEY` - Stripe secret key (sk_test_...)
- `STRIPE_PUBLISHABLE_KEY` - Stripe publishable key (pk_test_...)
- `STRIPE_WEBHOOK_SECRET` - Webhook signing secret (whsec_...)

Optional:
- `PORT` - Server port (default: 8080)
- `HOST` - Server host (default: localhost)
- `DB_PATH` - SQLite database path (default: ./car-shop.db)
