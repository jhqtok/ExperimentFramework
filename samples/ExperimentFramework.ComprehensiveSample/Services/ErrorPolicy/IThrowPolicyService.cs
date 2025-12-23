namespace ExperimentFramework.ComprehensiveSample.Services.ErrorPolicy;

/// <summary>
/// Service demonstrating OnErrorThrow policy - fails fast, no retry
/// </summary>
public interface IThrowPolicyService
{
    Task ProcessAsync();
}

public class StableImplementation : IThrowPolicyService
{
    public Task ProcessAsync()
    {
        Console.WriteLine("    → StableImplementation: Processing successfully");
        return Task.CompletedTask;
    }
}

public class UnstableImplementation : IThrowPolicyService
{
    public Task ProcessAsync()
    {
        Console.WriteLine("    → UnstableImplementation: About to throw...");
        throw new InvalidOperationException("Unstable implementation failed!");
    }
}
