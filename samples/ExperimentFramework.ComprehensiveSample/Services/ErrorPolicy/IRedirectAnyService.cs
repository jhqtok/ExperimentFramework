namespace ExperimentFramework.ComprehensiveSample.Services.ErrorPolicy;

/// <summary>
/// Service demonstrating OnErrorRedirectAndReplayAny policy - tries all trials until success
/// </summary>
public interface IRedirectAnyService
{
    Task<string> ProcessAsync();
}

public class PrimaryProvider : IRedirectAnyService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → PrimaryProvider: Throwing exception...");
        throw new TimeoutException("Primary provider timeout!");
    }
}

public class SecondaryProvider : IRedirectAnyService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → SecondaryProvider: Throwing exception...");
        throw new HttpRequestException("Secondary provider unavailable!");
    }
}

public class TertiaryProvider : IRedirectAnyService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → TertiaryProvider: Processing successfully!");
        return Task.FromResult("Result from tertiary provider (backup succeeded)");
    }
}
