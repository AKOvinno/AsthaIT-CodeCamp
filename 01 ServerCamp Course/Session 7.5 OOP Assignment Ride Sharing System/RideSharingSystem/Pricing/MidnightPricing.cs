public class MidnightPricing : IPricingStrategy
{
    public double CalculateFare(double distance, double baseFare)
    {
        return baseFare + (distance * 0.75);
    }

    public string GetStrategyName() => "Midnight Pricing";
}
