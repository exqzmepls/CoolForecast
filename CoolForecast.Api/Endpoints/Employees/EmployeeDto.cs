namespace CoolForecast.Api.Endpoints.Employees;

public class EmployeeDto
{
    public required Guid Id { get; init; }

    public required string PersonnelNumber { get; init; }

    public required string FirstName { get; init; }

    public required string SecondName { get; init; }

    public required string LastName { get; init; }

    public required Guid DepartmentId { get; init; }
}