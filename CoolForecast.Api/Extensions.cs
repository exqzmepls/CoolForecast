using CoolForecast.Api.Core;
using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api;

internal static class Extensions
{
    public static IHost ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
        return host;
    }
}