using CoolForecast.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CoolForecast.Api.Core;

internal static class Extensions
{
    public static NpgsqlConnection GetNpgsqlConnection(this ApplicationDbContext dbContext)
    {
        return dbContext.Database.GetDbConnection() as NpgsqlConnection ?? throw new InvalidCastException();
    }
}