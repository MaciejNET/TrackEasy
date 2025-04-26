using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Stations;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

public sealed class TrackEasyDbContext(DbContextOptions<TrackEasyDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<DiscountCode> DiscountCodes { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Operator> Operators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackEasyDbContext).Assembly);
    }
}