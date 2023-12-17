using System.Buffers;
using CoolForecast.Api.Core.Entities;
using Npgsql;

namespace CoolForecast.Api.Core;

public class TrainingRepository(
    ApplicationDbContext dbContext,
    ILogger<TrainingRepository> logger
)
{
    public async Task<Guid> AddAsync(
        Stream dataStream,
        Training training,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding training...");

        var dbConnection = dbContext.GetNpgsqlConnection();
        var largeObjectManager = new NpgsqlLargeObjectManager(dbConnection);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var oid = await largeObjectManager.CreateAsync(preferredOid: 0, cancellationToken);
        logger.LogDebug("Data oid: {DataOid}", oid);
        await using (var writer = await largeObjectManager.OpenReadWriteAsync(oid, cancellationToken))
        {
            using (var memoryOwner = MemoryPool<byte>.Shared.Rent(2048))
            {
                var totalBytes = 0;
                var buffer = memoryOwner.Memory;

                while (true)
                {
                    var bytesRead = await dataStream.ReadAsync(buffer, cancellationToken);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    await writer.WriteAsync(buffer, cancellationToken);
                    totalBytes += bytesRead;
                }

                logger.LogDebug("Data size: {DataSize} bytes", totalBytes);
            }
        }

        training.DataOid = oid;

        await dbContext.Trainings.AddAsync(training, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation("Training ({TrainingId}) is added", training.Id);

        return training.Id;
    }
}