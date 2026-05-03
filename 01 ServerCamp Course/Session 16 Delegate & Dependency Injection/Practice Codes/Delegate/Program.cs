// Operation Summation = Calculator.Sum;
// Console.WriteLine(Summation.Invoke(5, 10)); 

// // Func is a built-in delegate that can return a value. It can take up to 16 parameters, with the last parameter being the return type.
// Func<int, int, int> SummationFunc = Calculator.Sum;
// Console.WriteLine(SummationFunc.Invoke(5, 10));

// // Action is a built-in delegate that does not return a value.
// Action<string> printMessage = message => Console.WriteLine(message);
// printMessage.Invoke("Hello, World!");

// // Predicate is a built-in delegate that returns a boolean value. It takes one parameter and returns true or false based on a condition.
// Predicate<int> IsEven = num => num % 2 == 0;
// Console.WriteLine(IsEven.Invoke(4)); // True

// public delegate int Operation (int num1, int num2);

// public class Calculator
// {
//     public static int Sum(int a, int b)
//     {
//         return a + b;
//     }
// }
// ----------------------------------------------------------------
// Operation operations = Calculator.Sum;
// operations += Calculator.Subtraction;

// operations.Invoke(10, 5); // This will call both Sum and Subtraction methods.
// public delegate void Operation (int num1, int num2);

// public class Calculator
// {
//     public static void Sum(int a, int b)
//     {
//         Console.WriteLine($"Summation is : {a + b}");
//     }
//     public static void Subtraction(int a, int b)
//     {
//         Console.WriteLine($"Subtraction is : {a - b}");
//     }
// }

// ------------------Notifying with Event--------------------
var paymentService = new PaymentService();
paymentService.NotifyWithEvent += () => Console.WriteLine("Email send from Event");
paymentService.NotifyWithEvent += () => Console.WriteLine("SMS send from Event");
paymentService.ProcessWithEvent();

public delegate void Notify();
public class PaymentService
{
    public event Notify? NotifyWithEvent;
    public void ProcessWithEvent()
    {
        Console.WriteLine("Processing payment with Event...");
        NotifyWithEvent?.Invoke();
    }
}