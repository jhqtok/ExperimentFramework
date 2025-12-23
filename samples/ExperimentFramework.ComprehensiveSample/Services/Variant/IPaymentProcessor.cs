namespace ExperimentFramework.ComprehensiveSample.Services.Variant;

/// <summary>
/// Service demonstrating variant feature flags with multiple payment providers
/// </summary>
public interface IPaymentProcessor
{
    Task<string> ProcessPaymentAsync(decimal amount, string currency);
}

public class StripePaymentProcessor : IPaymentProcessor
{
    public async Task<string> ProcessPaymentAsync(decimal amount, string currency)
    {
        Console.WriteLine($"    → StripePaymentProcessor: Processing {amount} {currency}");
        await Task.Delay(50);
        return $"Payment processed via Stripe: {amount} {currency}";
    }
}

public class PayPalPaymentProcessor : IPaymentProcessor
{
    public async Task<string> ProcessPaymentAsync(decimal amount, string currency)
    {
        Console.WriteLine($"    → PayPalPaymentProcessor: Processing {amount} {currency}");
        await Task.Delay(50);
        return $"Payment processed via PayPal: {amount} {currency}";
    }
}

public class SquarePaymentProcessor : IPaymentProcessor
{
    public async Task<string> ProcessPaymentAsync(decimal amount, string currency)
    {
        Console.WriteLine($"    → SquarePaymentProcessor: Processing {amount} {currency}");
        await Task.Delay(50);
        return $"Payment processed via Square: {amount} {currency}";
    }
}
