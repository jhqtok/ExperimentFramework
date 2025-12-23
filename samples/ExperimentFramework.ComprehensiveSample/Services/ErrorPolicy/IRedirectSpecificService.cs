namespace ExperimentFramework.ComprehensiveSample.Services.ErrorPolicy;

/// <summary>
/// Service demonstrating OnErrorRedirectAndReplay policy - redirects to a specific fallback trial
/// </summary>
public interface IRedirectSpecificService
{
    Task<string> ProcessAsync();
}

public class PrimaryImplementation : IRedirectSpecificService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → PrimaryImplementation: Throwing exception...");
        throw new InvalidOperationException("Primary implementation failed!");
    }
}

public class SecondaryImplementation : IRedirectSpecificService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → SecondaryImplementation: Also throwing...");
        throw new InvalidOperationException("Secondary implementation also failed!");
    }
}

public class NoopDiagnosticsHandler : IRedirectSpecificService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → NoopDiagnosticsHandler (dedicated fallback): Recording failure and returning safe value");
        // In a real scenario, this would log diagnostics, emit metrics, etc.
        return Task.FromResult("Safe fallback response from diagnostics handler");
    }
}
