using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Core.Entities;

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<LayoffForecast> LayoffForecasts { get; set; } = null!;
    public DbSet<Forecast> Forecasts { get; set; } = null!;
    public DbSet<Training> Trainings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>()
            .HasIndex(e => e.Name)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.PersonnelNumber)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasMany<LayoffForecast>()
            .WithOne(x => x.Employee)
            .HasPrincipalKey(x => x.PersonnelNumber)
            .HasForeignKey(x => x.PersonnelNumber);

        modelBuilder.Entity<LayoffForecast>()
            .Property(f => f.Probability)
            .HasPrecision(5, 2);
    }
}