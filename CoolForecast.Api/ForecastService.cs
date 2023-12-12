using System.Buffers;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CoolForecast.Api;

internal sealed class ForecastService(
    ApplicationDbContext dbContext,
    ILogger<ForecastService> logger
)
{
    public async Task<ForecastResult> CreateForecastAsync(ForecastRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating forecast...");

        var database = dbContext.Database;

        var dbConnection = database.GetDbConnection() as NpgsqlConnection ?? throw new InvalidCastException();
        var largeObjectManager = new NpgsqlLargeObjectManager(dbConnection);

        await using (var transaction = await database.BeginTransactionAsync(cancellationToken))
        {
            var oid = await largeObjectManager.CreateAsync(preferredOid: 0, cancellationToken);
            logger.LogDebug("Content oid: {ContentOid}", oid);
            await using (var writer = await largeObjectManager.OpenReadWriteAsync(oid, cancellationToken))
            {
                using (var memoryOwner = MemoryPool<byte>.Shared.Rent(2048))
                {
                    var buffer = memoryOwner.Memory;

                    while (true)
                    {
                        var bytesRead = await request.InputData.ReadAsync(buffer, cancellationToken);
                        if (bytesRead == 0)
                        {
                            break;
                        }

                        logger.LogDebug("Read bytes: {ContentSize}", bytesRead);
                        await writer.WriteAsync(buffer, cancellationToken);
                    }
                }
            }

            // add new record

            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        // call cv-model API
        return new ForecastResult();
    }
}