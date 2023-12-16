using CoolForecast.Api.Core;
using CoolForecast.Api.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CoolForecast.Api.Endpoints.Trainings;

public sealed class TrainingEndpoints() : ApiModule("trainings", "Trainings")
{
    protected override void AddEndpoints(RouteGroupBuilder trainings)
    {
        trainings.MapPost("/", AddAsync).Accepts<IFormFile>("text/csv");
    }

    private static async Task<Results<Ok<Guid>, BadRequest>> AddAsync(
        HttpRequest request,
        [AsParameters] CreateTrainingParams parameters,
        TrainingRepository repository,
        ILogger<TrainingEndpoints> logger,
        CancellationToken cancellationToken)
    {
        var training = new Training
        {
            Id = Guid.NewGuid(),
            TimestampUtc = DateTime.UtcNow,
            Accuracy = parameters.Accuracy,
            Recall = parameters.Recall,
            F1Score = parameters.F1Score,
            MeanSquaredError = parameters.MeanSquaredError,
            R2 = parameters.R2,
            AucRoc = parameters.AucRoc,
            LogLoss = parameters.LogLoss
        };

        try
        {
            var trainingId = await repository.AddAsync(request.Body, training, cancellationToken);
            return TypedResults.Ok(trainingId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Training is not created");
            return TypedResults.BadRequest();
        }
    }
}