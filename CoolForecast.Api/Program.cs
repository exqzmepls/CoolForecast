using CoolForecast.Api;
using CoolForecast.Api.Core;
using CoolForecast.Api.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresql")));
builder.Services.AddScoped<ForecastService>();
builder.Services.AddScoped<ForecastRepository>();
builder.Services.AddScoped<SourceRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.MapUploadData();

app.MapDepartmentsCrud();

app.MapEmployeesCrud();

app.Run();