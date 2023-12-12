using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CoolForecast.Api;

internal sealed class ForecastService(
    ApplicationDbContext dbContext,
    ILogger<ForecastService> logger
)
{
    public Task<ForecastResult> CreateForecastAsync(ForecastRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating forecast...");

        var connection = dbContext.Database.GetDbConnection() as NpgsqlConnection;

        var manager = new NpgsqlLargeObjectManager(connection!);
        var oid = manager.Create();
        using (var transaction = dbContext.Database.BeginTransaction())
        {
            using (var writer = manager.OpenReadWrite(oid))
            {
                var buffer = new Span<byte>();
                var bytesRead = request.InputData.Read(buffer);

                writer.Write(buffer);
            }
            
            // add new record

            transaction.Commit();
        }

        // call cv-model API

        return Task.FromResult(new ForecastResult());
    }
}