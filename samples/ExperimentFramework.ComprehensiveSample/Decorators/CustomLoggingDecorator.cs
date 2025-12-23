using ExperimentFramework.Decorators;

namespace ExperimentFramework.ComprehensiveSample.Decorators;

/// <summary>
/// Custom decorator that logs detailed information about experiment invocations
/// </summary>
public class CustomLoggingDecorator : IExperimentDecorator
{
    public int Order => 3; // Execute last in the pipeline

    public async ValueTask<object?> InvokeAsync(
        InvocationContext context,
        Func<ValueTask<object?>> next)
    {
        var args = context.Arguments.Length > 0
            ? string.Join(", ", context.Arguments.Select(a => a?.ToString() ?? "null"))
            : "no args";

        Console.WriteLine($"    [CustomLogging] {context.ServiceType.Name}.{context.MethodName}({args}) using trial '{context.TrialKey}'");

        try
        {
            var result = await next();
            Console.WriteLine($"    [CustomLogging] Success → returned: {result?.ToString() ?? "null"}");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    [CustomLogging] Failed → {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// Factory for creating custom logging decorators
/// </summary>
public class CustomLoggingDecoratorFactory : IExperimentDecoratorFactory
{
    public IExperimentDecorator Create(IServiceProvider services)
        => new CustomLoggingDecorator();
}
