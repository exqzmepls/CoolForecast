namespace CoolForecast.Api.Core;

internal sealed class LayoffForecastService(
    IHttpClientFactory httpClientFactory,
    ILogger<LayoffForecastService> logger
)
{
    public async Task<IEnumerable<EmployeeLayoff>> GetLayoffForecastsAsync(
        Stream dataStream,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting layoff forecasts...");

        var httpClient = httpClientFactory.CreateClient("ml");
        var apiClient = new MlApiClient(httpClient);

        var fileParameter = new FileParameter(dataStream, "file.csv", "multipart/form-data");
        var apiResponse = await apiClient.PostAsync(fileParameter, cancellationToken);

        var result = apiResponse.Data.Select(d => new EmployeeLayoff(d.Id, d.Predict * 100));
        return result;
    }
}

public record EmployeeLayoff(string PersonnelNumber, double LayoffProbability);