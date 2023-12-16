using CoolForecast.Api.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Endpoints.Departments;

public sealed class DepartmentsEndpoints() : ApiModule("departments", "Departments")
{
    protected override void AddEndpoints(RouteGroupBuilder departments)
    {
        departments.MapGet("/", GetAllAsync);

        departments.MapPost("/", AddAsync);
    }

    private static async Task<Results<Ok<List<DepartmentDto>>, BadRequest>> GetAllAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var response = await dbContext.Departments
            .Select(department => new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name
            })
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<Guid>, BadRequest>> AddAsync(
        ApplicationDbContext dbContext,
        ILogger<DepartmentsEndpoints> logger,
        CreateDepartmentDto dto,
        CancellationToken cancellationToken)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
        };
        await dbContext.Departments.AddAsync(department, cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Department is not created");
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok(department.Id);
    }
}