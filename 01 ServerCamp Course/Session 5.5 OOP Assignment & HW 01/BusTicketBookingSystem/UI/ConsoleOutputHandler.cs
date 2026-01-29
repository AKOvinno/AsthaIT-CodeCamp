using System;
public class ConsoleOutputHandler
{
    public void PrintError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }
    public void PrintSectionHeader(string title)
    {
        Console.WriteLine($"\n---------- {title} ----------");
    }
    // Print success message
    public void PrintSuccess(string message)
    {
        Console.WriteLine(message);
    }
    // Print menu
    public void PrintMenu()
    {
        DisplayFormatter.PrintMenu();
    }
    // Print exit message
    public void PrintExitMessage()
    {
        Console.WriteLine("Thank you for using Bus Ticket Booking System!");
    }
    // Print invalid choice message
    public void PrintInvalidChoice()
    {
        Console.WriteLine("Invalid choice. Please try again.");
    }
    // Print invalid input message
    public void PrintInvalidInput(string fieldName)
    {
        Console.WriteLine($"Invalid {fieldName}.");
    }
}
