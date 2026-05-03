PaymentProcessor creditCardPayment = new CreditCardPaymentProcessor(100.0m, "1234-5678-9012-3456");
creditCardPayment.ProcessPayment();

PaymentProcessor payPalPayment = new PayPalPaymentProcessor(50.0m, "jPZlF@example.com");
payPalPayment.ProcessPayment();

PaymentProcessor bkashPayment = new BkashPaymentProcessor(75.0m, "01712345678");
bkashPayment.ProcessPayment();
