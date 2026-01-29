using System;
// It handles user input and parsing in the bus ticket booking system.
public class InputHandler
{
    // Methods to read and parse different types of input
    public bool TryParseInt(string prompt, out int result)
    {
        Console.Write(prompt);
        return int.TryParse(Console.ReadLine(), out result);
    }
    // Method to read and parse decimal input
    public bool TryParseDecimal(string prompt, out decimal result)
    {
        Console.Write(prompt);
        return decimal.TryParse(Console.ReadLine(), out result);
    }
    // Method to read and parse DateTime input
    public bool TryParseDateTime(string prompt, out DateTime result)
    {
        Console.Write(prompt);
        return DateTime.TryParse(Console.ReadLine(), out result);
    }
    // Method to read and parse TimeSpan input
    public bool TryParseTimeSpan(string prompt, out TimeSpan result)
    {
        Console.Write(prompt);
        return TimeSpan.TryParse(Console.ReadLine(), out result);
    }
    // Method to read a string input
    public string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
    // Overloaded method to read a string input and convert to uppercase if needed
    public string ReadString(string prompt, bool toUpper = false)
    {
        Console.Write(prompt);
        string input = Console.ReadLine()?.Trim();
        return toUpper && input != null ? input.ToUpper() : input;
    }
}
