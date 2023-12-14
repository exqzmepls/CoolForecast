namespace CoolForecast.Api.Core;

public sealed record EmployeeForecast(
    Guid Id,
    double LayoffProbability
);