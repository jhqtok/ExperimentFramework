namespace ExperimentFramework.ComprehensiveSample.Services.ErrorPolicy;

/// <summary>
/// Service demonstrating OnErrorRedirectAndReplayOrdered policy - tries ordered fallback trials
/// </summary>
public interface IRedirectOrderedService
{
    Task<string> ProcessAsync();
}

public class CloudDatabaseImplementation : IRedirectOrderedService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → CloudDatabaseImplementation: Connection timeout...");
        throw new TimeoutException("Cloud database connection timeout!");
    }
}

public class LocalCacheImplementation : IRedirectOrderedService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → LocalCacheImplementation: Cache miss...");
        throw new KeyNotFoundException("Data not found in local cache!");
    }
}

public class InMemoryCacheImplementation : IRedirectOrderedService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → InMemoryCacheImplementation: Returning cached data successfully");
        return Task.FromResult("Data from in-memory cache (3rd fallback succeeded)");
    }
}

public class StaticDataImplementation : IRedirectOrderedService
{
    public Task<string> ProcessAsync()
    {
        Console.WriteLine("    → StaticDataImplementation: Returning static fallback data");
        return Task.FromResult("Static fallback data (last resort)");
    }
}
