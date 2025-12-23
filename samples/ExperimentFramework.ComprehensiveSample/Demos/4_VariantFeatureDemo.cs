using ExperimentFramework.ComprehensiveSample.Services.Variant;

namespace ExperimentFramework.ComprehensiveSample.Demos;

/// <summary>
/// Demonstrates Variant Feature Flags (multi-variant experiments)
/// Requires Microsoft.FeatureManagement with variant support
/// </summary>
public class VariantFeatureDemo(IPaymentProcessor paymentProcessor)
{
    public async Task RunAsync()
    {
        Console.WriteLine("\n" + new string('=', 80));
        Console.WriteLine("DEMO 4: VARIANT FEATURE FLAGS");
        Console.WriteLine(new string('=', 80));

        Console.WriteLine("\nVariant Feature Flags enable:");
        Console.WriteLine("  - Multi-variant experiments (not just true/false)");
        Console.WriteLine("  - Percentage-based rollout (weighted distribution)");
        Console.WriteLine("  - A/B/C/... testing with multiple variants");
        Console.WriteLine("  - Integration with Microsoft.FeatureManagement variants");

        Console.WriteLine("\nConfigured in appsettings.json:");
        Console.WriteLine("  PaymentProviderVariant:");
        Console.WriteLine("    - stripe: 40% weight");
        Console.WriteLine("    - paypal: 40% weight");
        Console.WriteLine("    - square: 20% weight");

        Console.WriteLine("\nProcessing payment (variant selected based on configuration):");

        // The framework will use the variant feature flag to select a payment provider
        var result = await paymentProcessor.ProcessPaymentAsync(99.99m, "USD");

        Console.WriteLine($"  Result: {result}");
        Console.WriteLine("\n  → Variant determined by feature management configuration");
        Console.WriteLine("  → Different users may see different payment providers");
        Console.WriteLine("  → Supports gradual rollout and A/B/C testing");
    }
}
