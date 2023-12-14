namespace CoolForecast.Api.Core;

internal sealed class ForecastService(
    ApplicationDbContext dbContext,
    ILogger<ForecastService> logger
)
{
    private static DateTime _forecastTime = DateTime.UtcNow.Date;

    public Task<ForecastResult> GetForecastAsync(ForecastRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting forecast...");

        var result = GetForecast();

        logger.LogInformation("Forecast for {ForecastTime} is ready", result.TimeUtc);

        return Task.FromResult(result);
    }

    private ForecastResult GetForecast()
    {
        var employees = dbContext.Employees.ToList();
        var result = new ForecastResult(
            _forecastTime,
            employees.Select(e => new EmployeeForecast(e.Id, Random.Shared.NextDouble() * 100))
        );
        _forecastTime = _forecastTime.AddDays(1);
        return result;
    }
}