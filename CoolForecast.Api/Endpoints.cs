using CoolForecast.Api.Core;
using CoolForecast.Api.Core.Entities;
using CoolForecast.Api.Core.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api;

internal static class Endpoints
{
    public static IEndpointRouteBuilder MapUploadData(this IEndpointRouteBuilder app)
    {
        app.MapPost("/upload", UploadDataAsync)
            .Accepts<IFormFile>("text/csv");

        return app;
    }

    public static IEndpointRouteBuilder MapDepartmentsCrud(this IEndpointRouteBuilder app)
    {
        var departments = app.MapGroup("/departments");

        departments.MapGet("/", async (ApplicationDbContext db, CancellationToken tkn) =>
        {
            var response = db.Departments.Select(d => new
            {
                Id = d.Id,
                Name = d.Name
            });

            return await response.ToListAsync(tkn);
        });

        departments.MapPost("/", async (ApplicationDbContext db, DepartmentDto dto, CancellationToken tkn) =>
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };
            await db.Departments.AddAsync(department, tkn);
            await db.SaveChangesAsync(tkn);

            return department.Id;
        });

        return app;
    }

    public static IEndpointRouteBuilder MapEmployeesCrud(this IEndpointRouteBuilder app)
    {
        var departments = app.MapGroup("/employees");

        departments.MapGet("/", async (ApplicationDbContext db, CancellationToken tkn) =>
        {
            var response = db.Employees.Select(e => new
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentId = e.DepartmentId
            });

            return await response.ToListAsync(tkn);
        });

        departments.MapPost("/", async (ApplicationDbContext db, EmployeeDto dto, CancellationToken tkn) =>
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                DepartmentId = dto.DepartmentId
            };
            await db.Employees.AddAsync(employee, tkn);
            await db.SaveChangesAsync(tkn);

            return employee.Id;
        });

        return app;
    }

    private static async Task<Results<Ok, BadRequest>> UploadDataAsync(
        HttpRequest request,
        SourceRepository sourceRepository,
        ForecastService forecastService,
        ForecastRepository forecastRepository,
        CancellationToken cancellationToken)
    {
        var sourceId = await sourceRepository.AddSourceAsync(request.Body, cancellationToken);

        var forecastRequest = new ForecastRequest(sourceId);
        var forecast = await forecastService.GetForecastAsync(forecastRequest, cancellationToken);

        await forecastRepository.AddForecastAsync(forecast, cancellationToken);

        return TypedResults.Ok();
    }
}

public class DepartmentDto
{
    public required string Name { get; set; }
}

public class EmployeeDto
{
    public required string Name { get; set; }
    public required Guid DepartmentId { get; set; }
}