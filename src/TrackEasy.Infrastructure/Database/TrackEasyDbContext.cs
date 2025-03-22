using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

internal sealed class TrackEasyDbContext(DbContextOptions<TrackEasyDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackEasyDbContext).Assembly);
    }
}