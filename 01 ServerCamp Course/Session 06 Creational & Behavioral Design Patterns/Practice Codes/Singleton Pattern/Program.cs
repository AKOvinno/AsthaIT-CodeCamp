Whiteboard board1 = Whiteboard.GetInstance();
board1.Write("I want to learn C#");
board1.Print();

Whiteboard board2 = Whiteboard.GetInstance();
board2.Write("I want to learn Design Patterns");
board2.Print();
class Whiteboard
{
    private static Whiteboard? instance; // Eager Loading
    private static readonly object _lockObject = new object(); // Lock for thread safety
    public string Content = "";
    // We blocked creating of new instances from outside
    private Whiteboard()
    {
        
    }
    // We forced all users to use the same instance
    // Lazy initialization
    public static Whiteboard GetInstance() // Lazy loading
    {
        // helps in solving parallel thread issues
        lock (_lockObject) // two threads can't get here at the same time
        {
            if (instance == null)
            {
                instance = new Whiteboard();
            }
            return instance;
        }
    }
    // We shouldn't be able to create new instances
    // public static Whiteboard Create()
    // {
    //     return new Whiteboard();
    // }
    public void Write(string message)
    {
        Content += "\n" + message;
    }
    public void Print()
    {
        Console.WriteLine("Whiteboard Content:");
        Console.WriteLine(Content);
    }
}