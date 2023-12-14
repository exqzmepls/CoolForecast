using CoolForecast.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Core;

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<LayoffForecast> LayoffForecasts { get; set; } = null!;
    public DbSet<Source> Sources { get; set; } = null!;
}