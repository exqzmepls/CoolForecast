using System.ComponentModel.DataAnnotations;

namespace CoolForecast.Api.Core.Entities;

public sealed class Employee
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(16)]
    public string PersonnelNumber { get; set; } = null!;

    [MaxLength(32)]
    public string FirstName { get; set; } = null!;

    [MaxLength(32)]
    public string SecondName { get; set; } = null!;

    [MaxLength(32)]
    public string LastName { get; set; } = null!;

    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
}