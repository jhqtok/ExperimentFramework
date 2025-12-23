namespace ExperimentFramework.ComprehensiveSample.Services.Decorator;

/// <summary>
/// Service demonstrating custom decorators for cross-cutting concerns
/// </summary>
public interface IDataService
{
    Task<string> GetDataAsync(string key);
}

public class DatabaseDataService : IDataService
{
    public async Task<string> GetDataAsync(string key)
    {
        Console.WriteLine($"    → DatabaseDataService: Fetching '{key}' from database...");
        await Task.Delay(100); // Simulate database latency
        return $"Database data for '{key}'";
    }
}

public class CacheDataService : IDataService
{
    public async Task<string> GetDataAsync(string key)
    {
        Console.WriteLine($"    → CacheDataService: Fetching '{key}' from cache...");
        await Task.Delay(10); // Simulate faster cache access
        return $"Cached data for '{key}'";
    }
}
