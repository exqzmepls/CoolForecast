using System.ComponentModel.DataAnnotations;

namespace CoolForecast.Api.Endpoints.Employees;

public class CreateEmployeeDto
{
    [MaxLength(16)]
    public required string PersonnelNumber { get; set; }

    [MaxLength(32)]
    public required string FirstName { get; set; }

    [MaxLength(32)]
    public required string SecondName { get; set; }

    [MaxLength(32)]
    public required string LastName { get; set; }

    public required Guid DepartmentId { get; set; }
}