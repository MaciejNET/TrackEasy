using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Api.Endpoints;
using TrackEasy.Api.Filters;
using TrackEasy.Api.Swagger;
using TrackEasy.Application;
using TrackEasy.Infrastructure;
using TrackEasy.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.Development.json")
    .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Configuration.AddKeyVault();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAuthorizationHandlers()
    .AddTESwagger();

builder.Services.AddScoped<Global2FAFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseTESwagger();
}

app.UseInfrastructure();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.UseCors("AllowLocalhost5173");

app.MapControllers();

app.Run();

public partial class Program { }