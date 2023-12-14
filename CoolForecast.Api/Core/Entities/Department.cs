namespace CoolForecast.Api.Core.Entities;

public sealed class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public List<Employee>? Employees { get; set; }
}