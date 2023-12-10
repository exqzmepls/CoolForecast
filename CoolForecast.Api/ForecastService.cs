namespace CoolForecast.Api;

internal sealed class ForecastService(
    ILogger<ForecastService> logger
)
{
    public Task<ForecastResult> CreateForecastAsync(ForecastRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating forecast...");

        // call cv-model API

        return Task.FromResult(new ForecastResult());
    }
}