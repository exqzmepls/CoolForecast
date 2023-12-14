namespace CoolForecast.Api.Core;

public sealed record ForecastResult(
    DateTime TimeUtc,
    IEnumerable<EmployeeForecast> Employees
);