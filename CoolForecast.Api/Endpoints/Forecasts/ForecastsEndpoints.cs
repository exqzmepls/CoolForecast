using CoolForecast.Api.Core;
using CoolForecast.Api.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CoolForecast.Api.Endpoints.Forecasts;

public sealed class ForecastsEndpoints() : ApiModule("forecasts", "Forecasts")
{
    protected override void AddEndpoints(RouteGroupBuilder forecasts)
    {
        forecasts.MapPost("/", AddAsync).Accepts<IFormFile>("text/csv");
    }

    private static async Task<Results<Ok<Guid>, BadRequest>> AddAsync(
        HttpRequest request,
        ForecastRepository forecastRepository,
        IMediator mediator,
        ILogger<ForecastsEndpoints> logger,
        CancellationToken cancellationToken)
    {
        var dataUploadTimestampUtc = DateTime.UtcNow;
        var dataStream = request.Body;

        try
        {
            var forecastId = await forecastRepository.AddAsync(dataStream, dataUploadTimestampUtc, cancellationToken);
            var forecastCreatedEvent = new ForecastCreatedEvent
            {
                ForecastId = forecastId
            };
            await mediator.Publish(forecastCreatedEvent, cancellationToken);
            return TypedResults.Ok(forecastId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Forecast is not created");
            return TypedResults.BadRequest();
        }
    }
}