namespace CoolForecast.Api.Endpoints.Departments;

public class DepartmentDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}