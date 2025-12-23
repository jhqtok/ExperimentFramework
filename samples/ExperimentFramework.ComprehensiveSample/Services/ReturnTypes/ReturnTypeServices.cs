namespace ExperimentFramework.ComprehensiveSample.Services.ReturnTypes;

// ============================================================================
// 1. void Return Type
// ============================================================================

public interface IVoidService
{
    void Execute();
}

public class VoidImplementationA : IVoidService
{
    public void Execute()
    {
        Console.WriteLine("    → VoidImplementationA: Executing synchronously");
    }
}

public class VoidImplementationB : IVoidService
{
    public void Execute()
    {
        Console.WriteLine("    → VoidImplementationB: Executing synchronously");
    }
}

// ============================================================================
// 2. Task Return Type
// ============================================================================

public interface ITaskService
{
    Task ExecuteAsync();
}

public class TaskImplementationA : ITaskService
{
    public async Task ExecuteAsync()
    {
        Console.WriteLine("    → TaskImplementationA: Executing asynchronously");
        await Task.Delay(10);
    }
}

public class TaskImplementationB : ITaskService
{
    public async Task ExecuteAsync()
    {
        Console.WriteLine("    → TaskImplementationB: Executing asynchronously");
        await Task.Delay(10);
    }
}

// ============================================================================
// 3. Task<T> Return Type
// ============================================================================

public interface ITaskTService
{
    Task<string> GetResultAsync();
}

public class TaskTImplementationA : ITaskTService
{
    public async Task<string> GetResultAsync()
    {
        Console.WriteLine("    → TaskTImplementationA: Computing result");
        await Task.Delay(10);
        return "Result from TaskTImplementationA";
    }
}

public class TaskTImplementationB : ITaskTService
{
    public async Task<string> GetResultAsync()
    {
        Console.WriteLine("    → TaskTImplementationB: Computing result");
        await Task.Delay(10);
        return "Result from TaskTImplementationB";
    }
}

// ============================================================================
// 4. ValueTask Return Type
// ============================================================================

public interface IValueTaskService
{
    ValueTask ExecuteAsync();
}

public class ValueTaskImplementationA : IValueTaskService
{
    public async ValueTask ExecuteAsync()
    {
        Console.WriteLine("    → ValueTaskImplementationA: Executing with ValueTask");
        await Task.Delay(10);
    }
}

public class ValueTaskImplementationB : IValueTaskService
{
    public async ValueTask ExecuteAsync()
    {
        Console.WriteLine("    → ValueTaskImplementationB: Executing with ValueTask");
        await Task.Delay(10);
    }
}

// ============================================================================
// 5. ValueTask<T> Return Type
// ============================================================================

public interface IValueTaskTService
{
    ValueTask<int> GetResultAsync();
}

public class ValueTaskTImplementationA : IValueTaskTService
{
    public async ValueTask<int> GetResultAsync()
    {
        Console.WriteLine("    → ValueTaskTImplementationA: Computing result");
        await Task.Delay(10);
        return 42;
    }
}

public class ValueTaskTImplementationB : IValueTaskTService
{
    public async ValueTask<int> GetResultAsync()
    {
        Console.WriteLine("    → ValueTaskTImplementationB: Computing result");
        await Task.Delay(10);
        return 99;
    }
}
