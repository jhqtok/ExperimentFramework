using ExperimentFramework.ComprehensiveSample.Services.Decorator;

namespace ExperimentFramework.ComprehensiveSample.Demos;

/// <summary>
/// Demonstrates custom decorators for cross-cutting concerns:
/// - Timing/Performance measurement
/// - Caching
/// - Custom logging
/// - Retry logic
/// </summary>
public class CustomDecoratorDemo(IDataService dataService)
{
    public async Task RunAsync()
    {
        Console.WriteLine("\n" + new string('=', 80));
        Console.WriteLine("DEMO 2: CUSTOM DECORATORS");
        Console.WriteLine(new string('=', 80));

        Console.WriteLine("\nCustom decorators allow you to add cross-cutting concerns like:");
        Console.WriteLine("  - Performance timing");
        Console.WriteLine("  - Caching");
        Console.WriteLine("  - Custom logging");
        Console.WriteLine("  - Retry logic");
        Console.WriteLine("  - Request/response modification");

        Console.WriteLine("\nCalling _dataService.GetDataAsync(\"user123\"):");
        Console.WriteLine("  [All decorators are applied in order: Timing → Caching → Logging]");

        // First call - goes to actual implementation
        Console.WriteLine("\n  First call (cache miss):");
        var result1 = await dataService.GetDataAsync("user123");
        Console.WriteLine($"  Result: {result1}");

        // Second call - served from cache
        Console.WriteLine("\n  Second call (cache hit):");
        var result2 = await dataService.GetDataAsync("user123");
        Console.WriteLine($"  Result: {result2}");

        // Different key - cache miss again
        Console.WriteLine("\n  Third call with different key (cache miss):");
        var result3 = await dataService.GetDataAsync("user456");
        Console.WriteLine($"  Result: {result3}");

        Console.WriteLine("\n  → Decorators executed in order for each call");
        Console.WriteLine("  → Timing decorator measured total execution time");
        Console.WriteLine("  → Caching decorator prevented redundant calls");
        Console.WriteLine("  → Logging decorator tracked all invocations");
    }
}
