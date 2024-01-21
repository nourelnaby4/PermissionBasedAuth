using Microsoft.AspNetCore.Identity;
using PermissionBasedAuth.Constants;

namespace PermissionBasedAuth.Seeding
{
    public static class DefaultRoles
    {
        public static async Task CrateDefualtRoles(RoleManager<IdentityRole> _roleManager)
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(nameof(AppRoles.SuperAdmin)));
                await _roleManager.CreateAsync(new IdentityRole(nameof(AppRoles.Admin)));
                await _roleManager.CreateAsync(new IdentityRole(nameof(AppRoles.User)));
            }

        }
    }
}
