using CoolForecast.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ForecastService>();
builder.Services.AddScoped<ForecastRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapUploadData();

app.Run();