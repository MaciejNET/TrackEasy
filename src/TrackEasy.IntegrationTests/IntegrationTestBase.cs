using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using TrackEasy.Application;
using TrackEasy.Infrastructure;

namespace TrackEasy.IntegrationTests;

[Collection("Sequential")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected ISender Sender;
    protected readonly FakeTimeProvider TimeProvider = new(new DateTimeOffset(2025, 03, 23, 0, 0, 0, TimeSpan.Zero));
    protected readonly IServiceProvider ServiceProvider;
    
    private readonly DatabaseFixture _databaseFixture;
    
    public IntegrationTestBase(DatabaseFixture databaseFixture)
    {
        var services = new ServiceCollection();
        
        var configurationDictionary = new Dictionary<string, string>
        {
            {"ConnectionStrings:DefaultConnection", databaseFixture.ConnectionString}
        };
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationDictionary!)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);
        
        services
            .AddApplication()
            .AddInfrastructure();
        
        ConfigureServices(services);
        
        ServiceProvider = services.BuildServiceProvider();
        Sender = ServiceProvider.GetRequiredService<ISender>();
        _databaseFixture = databaseFixture;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider);
    }
    
    public async Task InitializeAsync()
        => await DatabaseCleaner.Clean(_databaseFixture.ConnectionString, "dbo");

    public Task DisposeAsync() => Task.CompletedTask;
}