using Microsoft.AspNetCore.Identity;
using PermissionBasedAuth.Constants;
using System.Reflection;
using System.Security.Claims;

namespace PermissionBasedAuth.Seeding
{
    public static class DefaultUsers
    {

        public static async Task CrateDefualtUser(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {

            if (!userManager.Users.Any())
            {
                var defaultUser = new IdentityUser
                {
                    UserName = "ahmed",
                    Email = "ahmed@gmail.com",
                    EmailConfirmed = true,
                };

                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user is null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "123456");
                    await userManager.AddToRoleAsync(defaultUser, nameof(AppRoles.SuperAdmin));
                    await roleManager.SeedClaimsForSuperUser();
                   
                }

            }

        }

        public static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(nameof(AppRoles.SuperAdmin));
            await roleManager.AddPermissionClaims(adminRole,nameof( Modules.General));
        }

        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permission.GenerateModuleClaimsList(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == nameof(ClaimType.Permission) && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(nameof(ClaimType.Permission), permission));
                }
            }
        }
    }
}
