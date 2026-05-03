public class RushHourPricing : IPricingStrategy
{
    public double CalculateFare(double distance, double baseFare)
    {
        return baseFare + (distance * 1.00);
    }

    public string GetStrategyName() => "Rush Hour Pricing";
}
