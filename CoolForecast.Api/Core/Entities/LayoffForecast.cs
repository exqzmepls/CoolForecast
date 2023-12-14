using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Core.Entities;

[Keyless]
public sealed class LayoffForecast
{
    public DateTimeOffset Time { get; set; }
    public double? Probability { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}