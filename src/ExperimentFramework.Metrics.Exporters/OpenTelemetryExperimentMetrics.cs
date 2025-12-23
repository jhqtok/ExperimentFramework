using ExperimentFramework.Metrics;
using System.Diagnostics.Metrics;

namespace ExperimentFramework.Metrics.Exporters;

/// <summary>
/// OpenTelemetry-compatible metrics exporter using System.Diagnostics.Metrics.
/// Integrates seamlessly with OpenTelemetry SDK and other observability platforms.
/// </summary>
public sealed class OpenTelemetryExperimentMetrics : IExperimentMetrics
{
    private readonly Meter _meter;
    private readonly Counter<long> _counter;
    private readonly Histogram<double> _histogram;

    /// <summary>
    /// Creates a new OpenTelemetry metrics exporter.
    /// </summary>
    /// <param name="meterName">The meter name for the metrics. Defaults to "ExperimentFramework".</param>
    /// <param name="version">The version of the metrics schema. Defaults to "1.0.0".</param>
    public OpenTelemetryExperimentMetrics(string meterName = "ExperimentFramework", string? version = "1.0.0")
    {
        _meter = new Meter(meterName, version);
        _counter = _meter.CreateCounter<long>("experiment_counter", description: "Experiment counter metrics");
        _histogram = _meter.CreateHistogram<double>("experiment_histogram", description: "Experiment histogram metrics");
    }

    public void IncrementCounter(string name, long value = 1, params KeyValuePair<string, object>[] tags)
    {
        _counter.Add(value, tags);
    }

    public void RecordHistogram(string name, double value, params KeyValuePair<string, object>[] tags)
    {
        _histogram.Record(value, tags);
    }

    public void SetGauge(string name, double value, params KeyValuePair<string, object>[] tags)
    {
        // OpenTelemetry gauges are created with observable callbacks
        // For dynamic gauge values, we use histogram as an approximation
        // Users should register ObservableGauge directly if they need true gauges
        _histogram.Record(value, tags);
    }

    public void RecordSummary(string name, double value, params KeyValuePair<string, object>[] tags)
    {
        // Summaries are represented as histograms in OpenTelemetry
        _histogram.Record(value, tags);
    }

    /// <summary>
    /// Disposes the underlying Meter.
    /// </summary>
    public void Dispose()
    {
        _meter.Dispose();
    }
}
