using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionBasedAuth.Context;
using PermissionBasedAuth.ViewModels;

namespace PermissionBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _conetxt;
        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager , ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _conetxt = context;
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUser()
        {

            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userModel = users.Select(user => new
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = _userManager.GetRolesAsync(user).Result
            });


            return Ok(userModel);
        }

        [HttpGet("manage-role")]
        public async Task<IActionResult> ManageRole(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles=await _roleManager.Roles.ToListAsync();

            var userModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(identityRole => new SelectedViewModel
                {
                    Lable = identityRole.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, identityRole.Name).Result
                }).ToList(),
            };
            return Ok(userModel);
        }

        [HttpPost]
        public async Task <IActionResult> AssignUserRoles(UserRoleViewModel userRoleModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleModel.UserId);
            if (user == null) return NotFound();

            var transaction = _conetxt.Database.BeginTransaction();

            try
            {

                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, userRoleModel.Roles.Where(x => x.IsSelected).Select(x => x.Lable));

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                return BadRequest(userRoleModel);
            }

            return Ok(userRoleModel);
        }
    }
}