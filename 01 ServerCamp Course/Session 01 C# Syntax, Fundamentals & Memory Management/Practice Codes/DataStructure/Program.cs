// See https://aka.ms/new-console-template for more information
Console.WriteLine("C# List");

List<int> nums = new List<int>();
nums.Add(5);
nums.Add(7);
nums.Add(10);
Console.WriteLine("Numbers Count: " + nums.Count);
nums.Remove(7);
Console.WriteLine("Numbers Count: " + nums.Count);
foreach(int num in nums)
{
    Console.WriteLine(num);
}
