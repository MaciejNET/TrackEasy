using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Api.Endpoints;
using TrackEasy.Api.Filters;
using TrackEasy.Api.Policies;
using TrackEasy.Api.Swagger;
using TrackEasy.Application;
using TrackEasy.Infrastructure;
using TrackEasy.Infrastructure.Services;
using TrackEasy.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Configuration.AddKeyVault();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPolicies()
    .AddAuthorizationHandlers()
    .AddTESwagger();

builder.Services.AddScoped<Global2FAFilter>();

var app = builder.Build();

app.MapOpenApi();
app.UseTESwagger();

app.UseInfrastructure();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notification");

app.MapEndpoints();

app.MapControllers();

app.Run();

public partial class Program { }