using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TrackEasy.Shared.Infrastructure;

namespace TrackEasy.Infrastructure.Database;

internal sealed class TrackEasyDbContextFactory : IDesignTimeDbContextFactory<TrackEasyDbContext>
{
    public TrackEasyDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TrackEasy.Api"))
            .AddJsonFile("appsettings.Development.json")
            .AddKeyVault()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<TrackEasyDbContext>();
        var connectionString = configuration.GetValue<string>("cs-application");

        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
            sqlOptions.MigrationsAssembly(typeof(TrackEasyDbContext).Assembly.FullName);
            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
        });

        return new TrackEasyDbContext(optionsBuilder.Options);
    }
}