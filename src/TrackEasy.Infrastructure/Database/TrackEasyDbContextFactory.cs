using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TrackEasy.Infrastructure.Database;

internal sealed class TrackEasyDbContextFactory : IDesignTimeDbContextFactory<TrackEasyDbContext>
{
    public TrackEasyDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TrackEasy.Api"))
            .AddJsonFile("appsettings.Development.json")
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<TrackEasyDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
            sqlOptions.MigrationsAssembly(typeof(TrackEasyDbContext).Assembly.FullName);
        });

        return new TrackEasyDbContext(optionsBuilder.Options);
    }
}