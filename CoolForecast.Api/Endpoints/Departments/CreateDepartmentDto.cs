using System.ComponentModel.DataAnnotations;

namespace CoolForecast.Api.Endpoints.Departments;

public class CreateDepartmentDto
{
    [MaxLength(64)]
    
    public required string Name { get; set; }
}