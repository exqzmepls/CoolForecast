using CoolForecast.Api.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CoolForecast.Api.Endpoints.Employees;

public sealed class EmployeesEndpoints() : ApiModule("employees", "Employees")
{
    protected override void AddEndpoints(RouteGroupBuilder employees)
    {
        employees.MapGet("/", GetAllAsync);

        employees.MapPost("/", AddAsync);
    }

    private static async Task<Results<Ok<List<EmployeeDto>>, BadRequest>> GetAllAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var response = await dbContext.Employees
            .Select(employee => new EmployeeDto
            {
                Id = employee.Id,
                PersonnelNumber = employee.PersonnelNumber,
                FirstName = employee.FirstName,
                SecondName = employee.SecondName,
                LastName = employee.LastName,
                DepartmentId = employee.DepartmentId
            })
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<Guid>, BadRequest>> AddAsync(
        ApplicationDbContext dbContext,
        ILogger<EmployeesEndpoints> logger,
        CreateEmployeeDto dto,
        CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            PersonnelNumber = dto.PersonnelNumber,
            FirstName = dto.FirstName,
            SecondName = dto.SecondName,
            LastName = dto.LastName,
            DepartmentId = dto.DepartmentId
        };
        await dbContext.Employees.AddAsync(employee, cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Employee is not created");
            return TypedResults.BadRequest();
        }

        return TypedResults.Ok(employee.Id);
    }
}