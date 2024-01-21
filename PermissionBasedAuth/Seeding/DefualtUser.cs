using Microsoft.AspNetCore.Identity;
using PermissionBasedAuth.Constants;

namespace PermissionBasedAuth.Seeding
{
    public class DefualtUser
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DefualtUser(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }

        public async Task CrateDefualtUser()
        {
            if (!_userManager.Users.Any())
            {
                var defaultUser = new IdentityUser
                {
                    UserName = "ahmed",
                    Email = "ahmed@gmail.com",
                    EmailConfirmed = true,
                };
                var password = "123456";

                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user is null)
                {
                    await _userManager.CreateAsync(defaultUser, password);
                    await _userManager.AddToRoleAsync(user, nameof(AppRoles.SuperAdmin));
                }


            }

        }
    }
}
