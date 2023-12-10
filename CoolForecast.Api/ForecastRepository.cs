namespace CoolForecast.Api;

internal sealed class ForecastRepository(
    ILogger<ForecastRepository> logger
)
{
    public Task SaveForecastAsync(ForecastResult forecastResult, CancellationToken cancellationToken)
    {
        logger.LogInformation("Saving forecast...");
        // save forecast to DB

        return Task.CompletedTask;
    }
}