using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

internal sealed class SeedData(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<TrackEasyDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        await context.Database.MigrateAsync(cancellationToken);

        List<string> roles = [Roles.Admin, Roles.Manager, Roles.Passenger];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var adminUser = User.CreateAdmin("Admin", "Admin", "admin@admin.com", new DateOnly(1990, 1, 1), TimeProvider.System);
        
        if (await userManager.FindByEmailAsync(adminUser.Email!) is null)
        {
            await userManager.CreateAsync(adminUser, "Admin1234!");
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
        
        var dbContext = scope.ServiceProvider.GetRequiredService<TrackEasyDbContext>();

        await dbContext.Database.ExecuteSqlAsync(
            $"""
             UPDATE AspNetUsers
             SET 
                 TwoFactorEnabled = 0,
                 EmailConfirmed = 1
             WHERE Email = 'admin@admin.com'
             """, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}