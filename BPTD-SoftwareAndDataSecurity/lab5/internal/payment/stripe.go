package payment

import (
	"fmt"
	"os"

	"github.com/stripe/stripe-go/v81"
	"github.com/stripe/stripe-go/v81/checkout/session"
	"github.com/stripe/stripe-go/v81/webhook"
)

type StripeClient struct {
	SecretKey      string
	PublishableKey string
	WebhookSecret  string
}

// NewStripeClient creates a new Stripe client
func NewStripeClient() *StripeClient {
	secretKey := os.Getenv("STRIPE_SECRET_KEY")
	publishableKey := os.Getenv("STRIPE_PUBLISHABLE_KEY")
	webhookSecret := os.Getenv("STRIPE_WEBHOOK_SECRET")

	stripe.Key = secretKey

	return &StripeClient{
		SecretKey:      secretKey,
		PublishableKey: publishableKey,
		WebhookSecret:  webhookSecret,
	}
}

// CreateCheckoutSession creates a Stripe checkout session
func (sc *StripeClient) CreateCheckoutSession(carModel string, amount int64, successURL, cancelURL string) (*stripe.CheckoutSession, error) {
	params := &stripe.CheckoutSessionParams{
		PaymentMethodTypes: stripe.StringSlice([]string{
			"card",
		}),
		LineItems: []*stripe.CheckoutSessionLineItemParams{
			{
				PriceData: &stripe.CheckoutSessionLineItemPriceDataParams{
					Currency: stripe.String("usd"),
					ProductData: &stripe.CheckoutSessionLineItemPriceDataProductDataParams{
						Name: stripe.String(fmt.Sprintf("Автомобіль: %s", carModel)),
					},
					UnitAmount: stripe.Int64(amount),
				},
				Quantity: stripe.Int64(1),
			},
		},
		Mode:       stripe.String(string(stripe.CheckoutSessionModePayment)),
		SuccessURL: stripe.String(successURL),
		CancelURL:  stripe.String(cancelURL),
	}

	s, err := session.New(params)
	if err != nil {
		return nil, fmt.Errorf("failed to create checkout session: %w", err)
	}

	return s, nil
}

// VerifyWebhookSignature verifies the webhook signature
func (sc *StripeClient) VerifyWebhookSignature(payload []byte, signature string) (stripe.Event, error) {
	event, err := webhook.ConstructEvent(payload, signature, sc.WebhookSecret)
	if err != nil {
		return event, fmt.Errorf("failed to verify webhook signature: %w", err)
	}
	return event, nil
}
