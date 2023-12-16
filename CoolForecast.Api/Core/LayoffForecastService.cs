using CoolForecast.Api.Core.Entities;

namespace CoolForecast.Api.Core;

internal sealed class LayoffForecastService(
    ApplicationDbContext dbContext,
    ILogger<LayoffForecastService> logger
)
{
    public Task<IEnumerable<EmployeeLayoff>> GetLayoffForecastsAsync(
        Stream dataStream,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting layoff forecasts...");

        //HttpClient.PostAsync("", new StreamContent(dataStream, 4096));

        var result = GetForecast();

        return Task.FromResult(result);
    }

    private IEnumerable<EmployeeLayoff> GetForecast()
    {
        var employees = dbContext.Employees.ToList();
        return employees.Select(e =>
        {
            var layoffProbability = Random.Shared.NextDouble() * 100;
            return new EmployeeLayoff(e.PersonnelNumber, layoffProbability);
        });
    }
}

public record EmployeeLayoff(string PersonnelNumber, double LayoffProbability);