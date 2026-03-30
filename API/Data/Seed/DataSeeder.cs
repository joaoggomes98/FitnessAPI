using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data;

public static class DataSeeder
{
    public static async Task SeedRolesAndSuperAdminAsync(IServiceProvider serviceProvider)
    {
        // Serviços necessários
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // 1️⃣ Criar roles se não existirem
        string[] roles = new[] { "SuperAdmin", "PT", "Client" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // 2️⃣ Criar SuperAdmin se não existir
        var adminEmail = "superadmin@pts.com";
        var superAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (superAdmin == null)
        {
            superAdmin = new AppUser
            {
                Name = "Super Admin",
                Email = adminEmail,
                UserName = adminEmail
            };

            var result = await userManager.CreateAsync(superAdmin, "SuperAdmin123!"); // senha segura de exemplo
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }
        }
    }
}