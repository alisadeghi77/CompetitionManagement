using CompetitionManagement.Domain.Constant;
using CompetitionManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CompetitionManagement.Infrastructure.Seed;

public static class SeedService
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        string[] roleNames = [RoleConstant.Admin, RoleConstant.Planner, RoleConstant.Coach, RoleConstant.Athlete];
        foreach (var roleName in roleNames)
            if (!await roleManager.RoleExistsAsync(roleName)) 
                await roleManager.CreateAsync(new IdentityRole(roleName));

        // Seed admin user
        var adminUser = ApplicationUser.Create("09363364928", "Admin", "Admin");
        var user = await userManager.FindByNameAsync(adminUser.UserName!);

        if (user == null)
        {
            var createUser = await userManager.CreateAsync(adminUser);
            if (createUser.Succeeded) 
                await userManager.AddToRoleAsync(adminUser, RoleConstant.Admin);
        }
    }
}