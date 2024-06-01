namespace app.Utilities;

using System.Diagnostics;
using System.Diagnostics.Metrics;

public class Instrumentation : IDisposable
{
    internal const string ActivitySourceName = "MudBlazor.Services";
    internal const string MeterName = "MudBlazor.Counter";
    private readonly Meter meter;

    public Instrumentation()
    {
        string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
        this.ActivitySource = new ActivitySource(ActivitySourceName, version);
        this.meter = new Meter(MeterName, version);
        this.ProductCounter = this.meter.CreateCounter<long>(
            "product.counter",
            description: "The product counter");
    }

    public ActivitySource ActivitySource { get; }

    public Counter<long> ProductCounter { get; }

    public void Dispose()
    {
        this.ActivitySource.Dispose();
        this.meter.Dispose();
    }
}
