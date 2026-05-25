using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StoRvStar.Models.Identity;

namespace StoRvStar.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var options = serviceProvider.GetRequiredService<IOptions<IdentitySeedOptions>>().Value;
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

        if (!options.Enabled)
        {
            logger.LogInformation("Identity seed is disabled.");
            return;
        }

        var roles = new[] { options.AdminRole, "Manager" }
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminPassword = options.AdminPassword;

        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            adminPassword = Environment.GetEnvironmentVariable("STORVSTAR_ADMIN_PASSWORD");
        }

        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogWarning("Admin seed password is not configured. Set IdentitySeed:AdminPassword or STORVSTAR_ADMIN_PASSWORD.");
            return;
        }

        await CreateUserIfNotExists(
            userManager,
            options.AdminUsername,
            options.AdminEmail,
            adminPassword,
            options.AdminRole,
            logger);
    }

    private static async Task CreateUserIfNotExists(
        UserManager<AppUser> userManager,
        string username,
        string email,
        string password,
        string role,
        ILogger logger)
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

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                logger.LogWarning("Cannot create seed user {Username}: {Errors}", username, errors);
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
