Console.WriteLine("Choose: 1. Bkash  2. Rocket  3. Card 4. CoD");
string choice = Console.ReadLine() ?? "CoD";
MakePayment(choice, 1000);
// This code demonstrates the Factory Pattern in C#. The MakePayment method is responsible for creating instances of different payment methods based on user input. However, this approach violates the Open/Closed Principle, as we need to modify the MakePayment method every time we want to add a new payment method.
// SRP (Single Responsibility Principle) is also violated here, as the MakePayment method is responsible for both creating payment method instances and processing payments. To adhere to these principles, we can implement a Factory class that creates instances of payment methods without modifying existing code when new payment methods are added.
// Factory Pattern will solve these issues by encapsulating the object creation logic in a separate class, allowing us to add new payment methods without changing existing code and keeping the responsibilities of classes focused and cohesive.

static void MakePayment(string method, decimal amount)
{
    // Here, we use the Factory to create an instance of the payment method based on the user's choice. This way, we adhere to the Open/Closed Principle and keep our code maintainable and extensible.
    IPaymentMethod paymentMethod = PaymentFactory.CreatePaymentMethod(method);
    paymentMethod.Pay(amount);
    // Without Factory Pattern, we break Open/Closed Principle
    // Also, this method has multiple responsibilities (violating SRP)
    /*
    if (method == "Bkash")
    {
        BkashPayment bkash = new BkashPayment();
        bkash.Pay(amount);
    }
    else if (method == "Rocket")
    {
        RocketPayment rocket = new RocketPayment();
        rocket.Pay(amount);
    }
    else if (method == "Card")
    {
        CardPayment card = new CardPayment();
        card.Pay(amount);
    }
    else if (method == "CoD")
    {
        Console.WriteLine($"💰 Paid {amount} Taka via Cash on Delivery");
    }
    else
    {
        Console.WriteLine("Invalid payment method");
    }
    */
}

// This is the Factory class that creates instances of payment methods based on the provided method name. It adheres to the Open/Closed Principle by allowing us to add new payment methods without modifying existing code, and it also helps to keep the responsibilities of classes focused and cohesive.
class PaymentFactory
{
    // This method creates and returns an instance of the appropriate payment method based on the input string. If the input does not match any known payment method, it throws an exception.
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