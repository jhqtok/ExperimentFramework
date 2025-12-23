using ExperimentFramework.Decorators;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;

namespace ExperimentFramework.Resilience;

/// <summary>
/// Factory for creating circuit breaker decorators using Polly.
/// </summary>
public sealed class CircuitBreakerDecoratorFactory : IExperimentDecoratorFactory
{
    private readonly CircuitBreakerDecorator _decorator;

    public CircuitBreakerDecoratorFactory(CircuitBreakerOptions options, ILoggerFactory? loggerFactory = null)
    {
        var opts = options ?? throw new ArgumentNullException(nameof(options));
        var logger = loggerFactory?.CreateLogger("ExperimentFramework.CircuitBreaker");
        _decorator = new CircuitBreakerDecorator(opts, logger);
    }

    public IExperimentDecorator Create(IServiceProvider serviceProvider)
    {
        // Return singleton instance to share circuit breaker state across all invocations
        return _decorator;
    }

    private sealed class CircuitBreakerDecorator : IExperimentDecorator
    {
        private readonly ResiliencePipeline _pipeline;
        private readonly CircuitBreakerOptions _options;
        private readonly ILogger? _logger;

        public CircuitBreakerDecorator(CircuitBreakerOptions options, ILogger? logger)
        {
            _options = options;
            _logger = logger;

            // Build Polly resilience pipeline with circuit breaker
            var pipelineBuilder = new ResiliencePipelineBuilder();

            // Use failure ratio if specified, otherwise use default 50%
            var failureRatio = options.FailureRatioThreshold ?? 0.5;

            pipelineBuilder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = failureRatio,
                MinimumThroughput = options.MinimumThroughput,
                SamplingDuration = options.SamplingDuration,
                BreakDuration = options.BreakDuration,
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                OnOpened = args =>
                {
                    logger?.LogWarning(
                        "Circuit breaker opened after failure ratio {FailureRatio:P} exceeded threshold",
                        failureRatio);
                    return default;
                },
                OnClosed = args =>
                {
                    logger?.LogInformation("Circuit breaker closed - normal operation resumed");
                    return default;
                },
                OnHalfOpened = args =>
                {
                    logger?.LogInformation("Circuit breaker half-open - testing recovery");
                    return default;
                }
            });

            _pipeline = pipelineBuilder.Build();
        }

        public async ValueTask<object?> InvokeAsync(
            InvocationContext context,
            Func<ValueTask<object?>> next)
        {
            try
            {
                // Execute through Polly circuit breaker
                return await _pipeline.ExecuteAsync(async ct =>
                {
                    return await next();
                }, CancellationToken.None);
            }
            catch (BrokenCircuitException ex)
            {
                _logger?.LogWarning(ex,
                    "Circuit breaker is open for {ServiceType}.{MethodName} trial={TrialKey}",
                    context.ServiceType.Name,
                    context.MethodName,
                    context.TrialKey);

                // Handle based on configured action
                if (_options.OnCircuitOpen == CircuitBreakerAction.ThrowException)
                {
                    throw new CircuitBreakerOpenException(
                        $"Circuit breaker is open for trial '{context.TrialKey}' of {context.ServiceType.Name}.{context.MethodName}",
                        ex);
                }
                else
                {
                    // For FallbackToDefault or FallbackToSpecificTrial, rethrow
                    // The error policy in the experiment framework will handle the fallback
                    throw;
                }
            }
        }
    }
}

/// <summary>
/// Exception thrown when a circuit breaker is open.
/// </summary>
public sealed class CircuitBreakerOpenException : Exception
{
    public CircuitBreakerOpenException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}
