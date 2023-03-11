
using eWallet.Models;
using Microsoft.AspNetCore.Identity;

public static class WebApplicationExtension
{
    public static async Task UseRoleInitializer(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var rolesManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                await RoleInitializer.InitializeAsync(userManager, rolesManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}

public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        string ownerEmail = "sarvar@solid.uz";
        string ownerPassword = "zaq1xsw2cde3";

        var roles = new Dictionary<string, string>()
        {
            { Role.User, "Пользователь" }
        };

        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role.Key))
                await roleManager.CreateAsync(new ApplicationRole(role.Key, role.Value));

        ApplicationUser user = await userManager.FindByNameAsync(ownerEmail);
        if (user == null) {
            user = new ApplicationUser() { LastName = "Mamatqulov", FirstName = "Sarvarbek", Email = ownerEmail, UserName = ownerEmail, EmailConfirmed = false, PhoneNumberConfirmed = false, TwoFactorEnabled = false, AccessFailedCount = 0, LockoutEnabled = true };
            await userManager.CreateAsync(user, ownerPassword);
        }        
        await userManager.AddToRoleAsync(user, Role.User);        
    }
}
