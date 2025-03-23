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

        List<string> roles = ["Admin", "Manager", "Passenger"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var adminUser = new User
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            TwoFactorEnabled = false,
        };
        
        adminUser.UpdatePersonalData("Admin", "Admin", new DateOnly(1990, 1, 1), TimeProvider.System);
        
        if (await userManager.FindByEmailAsync(adminUser.Email) == null)
        {
            await userManager.CreateAsync(adminUser, "Admin1234!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }       
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}