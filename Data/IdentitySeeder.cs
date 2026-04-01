using Microsoft.AspNetCore.Identity;
using StoRvStar.Models.Identity;

namespace StoRvStar.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Manager" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await CreateUserIfNotExists(userManager, "admin", "admin@storvstar.local", "Admin123!", "Admin");
        await CreateUserIfNotExists(userManager, "manager1", "manager1@storvstar.local", "Manager123!", "Manager");
        await CreateUserIfNotExists(userManager, "manager2", "manager2@storvstar.local", "Manager123!", "Manager");
        await CreateUserIfNotExists(userManager, "manager3", "manager3@storvstar.local", "Manager123!", "Manager");
    }

    private static async Task CreateUserIfNotExists(
        UserManager<AppUser> userManager,
        string username,
        string email,
        string password,
        string role)
    {
        var user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            user = new AppUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}