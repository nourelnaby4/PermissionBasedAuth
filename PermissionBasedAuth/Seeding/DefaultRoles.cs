using Microsoft.AspNetCore.Identity;
using PermissionBasedAuth.Constants;

namespace PermissionBasedAuth.Seeding
{
    public class DefaultRoles
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public DefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            _roleManager=roleManager;
        }

        public async Task CrateDefualtRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                _roleManager.CreateAsync(new IdentityRole(nameof(AppRoles.SuperAdmin)));
                _roleManager.CreateAsync(new IdentityRole(nameof(AppRoles.Admin)));
                _roleManager.CreateAsync(new IdentityRole(nameof(AppRoles.User)));
            }
               
        }
    }
}
