using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Core.Entities;

[Keyless]
public sealed class LayoffForecast
{
    public DateTimeOffset Time { get; set; }

    public double? Probability { get; set; }

    public string PersonnelNumber { get; set; } = null!;
    public Employee? Employee { get; set; }

    public Guid ForecastId { get; set; }
    public Forecast? Forecast { get; set; }
}