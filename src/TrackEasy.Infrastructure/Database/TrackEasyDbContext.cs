using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Coaches;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Managers;
using TrackEasy.Domain.Notifications;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.RefundRequests;
using TrackEasy.Domain.Stations;
using TrackEasy.Domain.Tickets;
using TrackEasy.Domain.Trains;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

public sealed class TrackEasyDbContext(DbContextOptions<TrackEasyDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<DiscountCode> DiscountCodes { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<RefundRequest> RefundRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackEasyDbContext).Assembly);
    }
}