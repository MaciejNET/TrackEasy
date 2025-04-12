using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Users;
using TrackEasy.Domain.Stations;
using TrackEasy.Infrastructure.Stations.Cities.Configuration;
using TrackEasy.Domain.Entities;



namespace TrackEasy.Infrastructure.Database;

public sealed class TrackEasyDbContext(DbContextOptions<TrackEasyDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<DiscountCode> DiscountCodes { get; set; }
    public DbSet<City> Cities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackEasyDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new CityConfiguration());
    }
}