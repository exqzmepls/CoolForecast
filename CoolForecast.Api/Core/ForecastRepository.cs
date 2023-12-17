using System.Buffers;
using CoolForecast.Api.Core.Entities;
using Npgsql;

namespace CoolForecast.Api.Core;

internal sealed class ForecastRepository(
    ApplicationDbContext dbContext,
    ILogger<ForecastRepository> logger
)
{
    public async Task<Guid> AddAsync(
        Stream dataStream,
        DateTime dataUploadTimestampUtc,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding forecast...");

        var dbConnection = dbContext.GetNpgsqlConnection();
        var largeObjectManager = new NpgsqlLargeObjectManager(dbConnection);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var oid = await largeObjectManager.CreateAsync(preferredOid: 0, cancellationToken);
        logger.LogDebug("Data oid: {DataOid}", oid);
        await using (var writer = await largeObjectManager.OpenReadWriteAsync(oid, cancellationToken))
        {
            var buffer = new byte[2048];
            var totalBytes = 0;

            while (true)
            {
                var bytesRead = await dataStream.ReadAsync(buffer, 0, 2048, cancellationToken);
                logger.LogDebug("Bytes read: {BytesRead}", bytesRead);
                if (bytesRead == 0)
                {
                    break;
                }

                await writer.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                logger.LogDebug("Position: {Position}", writer.Position);
                totalBytes += bytesRead;
            }

            logger.LogDebug("Data size: {DataSize} bytes", totalBytes);
        }

        var forecast = new Forecast
        {
            Id = Guid.NewGuid(),
            DataUploadTimestampUtc = dataUploadTimestampUtc,
            DataOid = oid
        };
        await dbContext.Forecasts.AddAsync(forecast, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation("Forecast ({ForecastId}) is added", forecast.Id);

        return forecast.Id;
    }
}