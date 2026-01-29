public static class ValidationHelper
{
    
    public static bool IsNullOrEmpty(string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
    public static bool IsPositiveNumber(decimal number)
    {
        return number > 0;
    }
}
