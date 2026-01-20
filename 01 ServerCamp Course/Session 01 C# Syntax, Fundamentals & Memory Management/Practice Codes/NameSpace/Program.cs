using System.Net;
using HelperMethods;

//Utility.Greet("Ashfaq Kadir Ovinno");
// Accessing non-static method Greet() after creating Instance
Utility util = new Utility();
util.Greet("Ashfaq");
// Accessing static method Add() directly
Console.WriteLine(Utility.Add(6, 7));