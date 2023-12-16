using CoolForecast.Api.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CoolForecast.Api.Core.Notifications;

internal sealed class ForecastCreatedEventHandler(
        ApplicationDbContext dbContext,
        LayoffForecastService layoffForecastService,
        ILogger<ForecastCreatedEventHandler> logger
    )
    : INotificationHandler<ForecastCreatedEvent>
{
    public async Task Handle(ForecastCreatedEvent notification, CancellationToken cancellationToken)
    {
        var dbConnection = dbContext.GetNpgsqlConnection();
        var largeObjectManager = new NpgsqlLargeObjectManager(dbConnection);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var forecast = await dbContext.Forecasts.SingleAsync(f => f.Id == notification.ForecastId, cancellationToken);
        await using var dataStream = await largeObjectManager.OpenReadAsync(forecast.DataOid, cancellationToken);

        var layoffForecasts = await layoffForecastService.GetLayoffForecastsAsync(dataStream, cancellationToken);
        await AddLayoffForecastsAsync(forecast, layoffForecasts, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task AddLayoffForecastsAsync(
        Forecast forecast,
        IEnumerable<EmployeeLayoff> layoffs,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding layoff forecasts...");

        var values = layoffs.Select(l =>
            $"('{forecast.DataUploadTimestampUtc}', '{forecast.Id}', '{l.PersonnelNumber}', {l.LayoffProbability:F})");
        var sql =
            $"INSERT INTO \"LayoffForecasts\" (\"Time\", \"ForecastId\", \"PersonnelNumber\", \"Probability\") VALUES {string.Join(',', values)};";

        await dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);

        logger.LogInformation("Layoff forecasts are added");
    }
}