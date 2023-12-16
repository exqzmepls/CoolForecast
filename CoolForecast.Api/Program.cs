using Carter;
using CoolForecast.Api;
using CoolForecast.Api.Core;
using CoolForecast.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresql")));
builder.Services.AddScoped<LayoffForecastService>();
builder.Services.AddScoped<ForecastRepository>();
builder.Services.AddCarter();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();

app.UseSerilogRequestLogging();

app.MapCarter();

app.Run();