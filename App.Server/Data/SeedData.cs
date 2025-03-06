using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using App.Shared.Models;
using App.Shared.Responses;

public static class SeedData
{
    public static async Task<ServiceResponse<string>> Initialize(IServiceProvider serviceProvider)
    {
        var response = new ServiceResponse<string>();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = { "Admin", "Librarian", "Member" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create Admin User
        var adminUser = new ApplicationUser
        {
            UserName = "admin@library.com",
            Email = "admin@library.com"
        };

        string adminPassword = "Admin@123";
        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                response.Success = true;
                response.Message = "Database seeded successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to create admin user";
                response.Data = string.Join(", ", createAdmin.Errors.Select(e => e.Description));
            }
        }
        else
        {
            response.Success = true;
            response.Message = "Database already seeded";
        }

        return response;
    }
}