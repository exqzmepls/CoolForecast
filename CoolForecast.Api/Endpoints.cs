using Microsoft.AspNetCore.Http.HttpResults;

namespace CoolForecast.Api;

internal static class Endpoints
{
    public static IEndpointRouteBuilder MapUploadData(this IEndpointRouteBuilder app)
    {
        app.MapPost("/upload", UploadDataAsync)
            .Accepts<IFormFile>("text/csv");

        return app;
    }

    private static async Task<Results<Ok, BadRequest>> UploadDataAsync(
        HttpRequest request,
        ForecastService service,
        ForecastRepository repository,
        CancellationToken cancellationToken)
    {
        var forecastRequest = new ForecastRequest(request.Body);
        var forecast = await service.CreateForecastAsync(forecastRequest, cancellationToken);

        await repository.SaveForecastAsync(forecast, cancellationToken);

        return TypedResults.Ok();
    }
}