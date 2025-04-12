using Scalar.AspNetCore;
using TrackEasy.Api.Endpoints;
using TrackEasy.Application;
using TrackEasy.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.Development.json")
    .AddEnvironmentVariables();

builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddInfrastructure();

builder.Services.AddScoped<ICityRepository, CityRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.DarkMode = true;
    });
}

app.UseInfrastructure();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.MapControllers();

app.Run();

public partial class Program { }