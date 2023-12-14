using CoolForecast.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Core.Repositories;

internal sealed class ForecastRepository(
    ApplicationDbContext applicationDbContext,
    ILogger<ForecastRepository> logger
)
{
    public async Task AddForecastAsync(ForecastResult forecastResult, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding layoff forecasts...");

        var layoffForecast = forecastResult.Employees
            .Select(employee => new LayoffForecast
            {
                Time = forecastResult.TimeUtc,
                Probability = employee.LayoffProbability,
                EmployeeId = employee.Id,
            });

        var values = layoffForecast.Select(l => $"('{l.Time}', '{l.EmployeeId}', {l.Probability})");
        var sql =
            $"INSERT INTO \"LayoffForecasts\" (\"Time\", \"EmployeeId\", \"Probability\") VALUES {string.Join(',', values)};";

        await applicationDbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);

        logger.LogInformation("Layoff forecasts are added");
    }
}