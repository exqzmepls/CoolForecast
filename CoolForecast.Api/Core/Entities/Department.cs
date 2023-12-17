using System.ComponentModel.DataAnnotations;

namespace CoolForecast.Api.Core.Entities;

public sealed class Department
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; } = null!;

    public IEnumerable<Employee>? Employees { get; set; }
}