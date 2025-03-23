using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

public sealed class TrackEasyDbContext(DbContextOptions<TrackEasyDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<DiscountCode> DiscountCodes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackEasyDbContext).Assembly);
    }
}