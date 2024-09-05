﻿using Microsoft.AspNetCore.Identity;
using MyApp.Models;

public static class SeedData
{
    public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Create roles if they don't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Create admin user
        string adminEmail = "sarath@example.com";
        string adminPassword = "Pa$$w0rd"; // Admin password
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin User",
                Address = "Admin Address",
                MobileNumber = "123456789",
                ZodiacSign = "Leo"
            };

            var result = await userManager.CreateAsync(adminUser, "Pa$$w0rd");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
