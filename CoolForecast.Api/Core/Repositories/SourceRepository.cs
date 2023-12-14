using System.Buffers;
using CoolForecast.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CoolForecast.Api.Core.Repositories;

internal sealed class SourceRepository(
    ApplicationDbContext dbContext,
    ILogger<SourceRepository> logger
)
{
    public async Task<Guid> AddSourceAsync(Stream sourceDataStream, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding source...");

        var database = dbContext.Database;

        var dbConnection = database.GetDbConnection() as NpgsqlConnection ?? throw new InvalidCastException();
        var largeObjectManager = new NpgsqlLargeObjectManager(dbConnection);

        await using var transaction = await database.BeginTransactionAsync(cancellationToken);
        var oid = await largeObjectManager.CreateAsync(preferredOid: 0, cancellationToken);
        logger.LogDebug("Source data oid: {SourceDataOid}", oid);
        await using (var writer = await largeObjectManager.OpenReadWriteAsync(oid, cancellationToken))
        {
            using (var memoryOwner = MemoryPool<byte>.Shared.Rent(2048))
            {
                var totalBytes = 0;
                var buffer = memoryOwner.Memory;

                while (true)
                {
                    var bytesRead = await sourceDataStream.ReadAsync(buffer, cancellationToken);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    await writer.WriteAsync(buffer, cancellationToken);
                    totalBytes += bytesRead;
                }

                logger.LogDebug("Source size: {SourceSize} bytes", totalBytes);
            }
        }

        var source = new Source
        {
            Id = Guid.NewGuid(),
            TimestampUtc = DateTime.UtcNow,
            DataOid = oid
        };
        await dbContext.Sources.AddAsync(source, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation("Source ({SourceId}) is added", source.Id);

        return source.Id;
    }
}