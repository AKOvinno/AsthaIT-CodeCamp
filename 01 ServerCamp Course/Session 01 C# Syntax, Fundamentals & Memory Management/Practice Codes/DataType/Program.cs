// See https://aka.ms/new-console-template for more information
string name = "Ashfaq Kadir Ovinno";

// ------------- Example one ----------------
string Multiline = "Line one.\nLine two.";
Console.WriteLine(Multiline);

// -------------- Example two ---------------
string MultilineWithDollar = $"Hello, {name}, Welcome!";
Console.WriteLine(MultilineWithDollar);

// -------------- Example three -------------
string interpolatedVerbatim  = $@"Hello, {name}, Welcome!";
Console.WriteLine(interpolatedVerbatim );

// -------------- Example four --------------
double price = 19.99;
string interpolatedRaw = $$"""
{
  "item": "Widget",
  "price": {{price}} 
}
""";
Console.WriteLine(interpolatedRaw);
// The use of $$ allows for embedding { and } literally, 
// while {{ and }} are used for interpolation.

// -------------- Example five --------------
string json = """
{
  "name": "John Doe",
  "age": 30
}
""";
Console.WriteLine(json);