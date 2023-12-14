namespace CoolForecast.Api.Core.Entities;

public sealed class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}