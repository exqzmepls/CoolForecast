using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}