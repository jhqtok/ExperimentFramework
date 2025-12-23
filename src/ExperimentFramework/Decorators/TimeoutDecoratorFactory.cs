using ExperimentFramework.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExperimentFramework.Decorators;

/// <summary>
/// Factory for creating timeout enforcement decorators.
/// </summary>
public sealed class TimeoutDecoratorFactory : IExperimentDecoratorFactory
{
    private readonly TimeoutPolicy _policy;
    private readonly ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeoutDecoratorFactory"/> class.
    /// </summary>
    /// <param name="policy">The timeout policy to enforce.</param>
    /// <param name="loggerFactory">Optional logger factory for logging timeout events.</param>
    public TimeoutDecoratorFactory(TimeoutPolicy policy, ILoggerFactory? loggerFactory = null)
    {
        _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        _loggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public IExperimentDecorator Create(IServiceProvider serviceProvider)
    {
        var logger = _loggerFactory?.CreateLogger("ExperimentFramework.Timeout");
        return new TimeoutDecorator(_policy, logger);
    }

    private sealed class TimeoutDecorator : IExperimentDecorator
    {
        private readonly TimeoutPolicy _policy;
        private readonly ILogger? _logger;

        public TimeoutDecorator(TimeoutPolicy policy, ILogger? logger)
        {
            _policy = policy;
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(
            InvocationContext context,
            Func<ValueTask<object?>> next)
        {
            using var cts = new CancellationTokenSource(_policy.Timeout);

            try
            {
                var task = next().AsTask();
                var completedTask = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cts.Token));

                if (completedTask == task)
                {
                    // Trial completed within timeout
                    return await task;
                }
                else
                {
                    // Timeout occurred
                    var timeoutEx = new TimeoutException(
                        $"Trial '{context.TrialKey}' for {context.ServiceType.Name}.{context.MethodName} " +
                        $"exceeded timeout of {_policy.Timeout.TotalMilliseconds}ms");

                    _logger?.LogWarning(timeoutEx,
                        "Trial timeout: {ServiceType}.{MethodName} trial={TrialKey} timeout={TimeoutMs}ms",
                        context.ServiceType.Name,
                        context.MethodName,
                        context.TrialKey,
                        _policy.Timeout.TotalMilliseconds);

                    throw timeoutEx;
                }
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
            {
                // Convert to TimeoutException
                var timeoutEx = new TimeoutException(
                    $"Trial '{context.TrialKey}' for {context.ServiceType.Name}.{context.MethodName} " +
                    $"exceeded timeout of {_policy.Timeout.TotalMilliseconds}ms");

                _logger?.LogWarning(timeoutEx,
                    "Trial timeout (cancellation): {ServiceType}.{MethodName} trial={TrialKey} timeout={TimeoutMs}ms",
                    context.ServiceType.Name,
                    context.MethodName,
                    context.TrialKey,
                    _policy.Timeout.TotalMilliseconds);

                throw timeoutEx;
            }
        }
    }
}
