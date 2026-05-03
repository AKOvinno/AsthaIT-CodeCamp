ShoppingCart cart = new ShoppingCart();
// Here, shopping card created once and payment method set dynamically at runtime. This is the essence of the Strategy Pattern, which allows us to change the behavior of an object (in this case, the payment method) without modifying its code. The Factory Pattern is used to create instances of payment methods based on user input, further enhancing the flexibility and maintainability of our code.
// Cart is not creating but it's changing its behavior dynamically at runtime by setting different payment methods. This is the core idea of the Strategy Pattern, which promotes composition over inheritance and allows us to change the behavior of an object without modifying its code. The Factory Pattern is used to create instances of payment methods based on user input, further enhancing the flexibility and maintainability of our code.
while (true)
{
    Console.WriteLine("\n=== Choose Payment ===");
    Console.WriteLine("1. Bkash");
    Console.WriteLine("2. Nagad");
    Console.WriteLine("3. Card");
    Console.WriteLine("4. Checkout");
    Console.WriteLine("0. Exit");

    string choice = Console.ReadLine();

    IPaymentMethod? paymentMethod = null;

    // Factory Pattern + Strategy Pattern
    // Here, we use the Factory to create an instance of the payment method based on the user's choice. This way, we adhere to the Open/Closed Principle and keep our code maintainable and extensible. The Strategy Pattern allows us to set the payment method dynamically at runtime without modifying the ShoppingCart class.
    switch (choice)
    {
        case "1":
            // Factory Pattern
            paymentMethod = PaymentFactory.CreatePaymentMethod("Bkash");
            // Strategy Pattern: sets payment method dynamically at runtime
            cart.SetPaymentMethod(paymentMethod);
            break;
        case "2":
            paymentMethod = PaymentFactory.CreatePaymentMethod("Rocket");
            cart.SetPaymentMethod(paymentMethod);
            break;
        case "3":
            paymentMethod = PaymentFactory.CreatePaymentMethod("Card"); // Factory Pattern
            cart.SetPaymentMethod(paymentMethod); // Strategy Pattern
            break;
        case "4":
            cart.Checkout(500);
            break;
        case "0":
            return;
    }
}
// This code demonstrates the Strategy Pattern in C#. The ShoppingCart class has a SetPaymentMethod method that allows us to set the payment method dynamically at runtime. Each payment method is encapsulated in its own class that implements the IPaymentMethod interface, allowing for flexibility and adherence to the Open/Closed Principle. The Factory Pattern is also used to create instances of payment methods based on user input, further enhancing the maintainability and extensibility of the code.
// In ShoppingCart, nothing is created but the behavior is changing dynamically at runtime by setting different payment methods. This is the core idea of the Strategy Pattern, which promotes composition over inheritance and allows us to change the behavior of an object without modifying its code. The Factory Pattern is used to create instances of payment methods based on user input, further enhancing the flexibility and maintainability of our code.
class ShoppingCart
{
    private IPaymentMethod? _paymentMethod = null; //bkash, card

    // Strategy Pattern
    public void SetPaymentMethod(IPaymentMethod paymentMethod) // BkashPayment
    {
        Console.WriteLine($"✅ Payment method set to {paymentMethod.GetType().Name}");
        _paymentMethod = paymentMethod;
    }
    // Here, SetPaymentMethod is continuously changing the behavior of the ShoppingCart by setting different payment methods at runtime. This is the essence of the Strategy Pattern, which allows us to change the behavior of an object without modifying its code. The Factory Pattern is used to create instances of payment methods based on user input, further enhancing the flexibility and maintainability of our code.
    // In Factory Pattern, we created the payment method instances based on user input, and in Strategy Pattern, we set those payment methods dynamically at runtime without modifying the ShoppingCart class. This combination of patterns allows us to create a flexible and maintainable codebase that adheres to SOLID principles.

    public void Checkout(decimal amount)
    {
        if (_paymentMethod == null)
        {
            Console.WriteLine("❌ Payment method not set!");
            return;
        }
        _paymentMethod.Pay(amount); //card
    }
}

// == Factory Pattern Implementation ==

class PaymentFactory
{
    public static IPaymentMethod CreatePaymentMethod(string method)
    {
        if (method == "Bkash")
        {
            return new BkashPayment();
        }
        else if (method == "Rocket")
        {
            return new RocketPayment();
        }
        else if (method == "Card")
        {
            return new CardPayment();
        }
        else if (method == "CoD")
        {
            return new CashOnDelivery();
        }
        else
        {
            throw new ArgumentException("Invalid payment method");
        }
    }
}

interface IPaymentMethod
{
    void Pay(decimal amount);
}

class CashOnDelivery : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Cash on Delivery");
    }
}

class BkashPayment : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Bkash");
    }
}

class RocketPayment : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Rocket");
    }
}

class CardPayment : IPaymentMethod
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Card");
    }
}