namespace ExperimentFramework.ComprehensiveSample.Services.ErrorPolicy;

/// <summary>
/// Service demonstrating OnErrorRedirectAndReplayDefault policy - falls back to default
/// </summary>
public interface IRedirectDefaultService
{
    Task<string> ProcessAsync();
}

public class DefaultImplementation : IRedirectDefaultService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → DefaultImplementation (fallback): Processing successfully");
        return Task.FromResult("Result from default implementation (stable fallback)");
    }
}

public class ExperimentalImplementation : IRedirectDefaultService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → ExperimentalImplementation: Throwing exception...");
        throw new NotImplementedException("Experimental feature not ready yet!");
    }
}
