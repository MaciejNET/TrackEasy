using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackEasy.Domain.Users;

namespace TrackEasy.Infrastructure.Database;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<TrackEasyDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

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
}