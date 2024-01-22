using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionBasedAuth.Constants;
using PermissionBasedAuth.Context;
using PermissionBasedAuth.ViewModels;
using System.Security.Claims;

namespace PermissionBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RolesController(RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        [HttpGet("get-roles")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleManager.Roles.Select(x => new { x.Id, x.Name }).ToListAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName)) { return BadRequest("role is existed alread"); }
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return Ok(roleName);
        }

        [HttpGet("get-role-permissions")]
        public async Task<IActionResult> GetRolePermission(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound("role is not found"); }
            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(x => x.Value);
            var allClaims = Permission.GenerateAllModuleClaims();

            var permissionRole = new PermissionRoleViewModel()
            {
                RoleId = roleId,
                RoleName = role.Name,
            };
            foreach (var claim in allClaims)
            {
                if (roleClaims.Any(x => x.Equals(claim)))
                {
                    permissionRole.Claims.Add(new SelectedViewModel { Lable = claim, IsSelected = true });
                    continue;
                }
                permissionRole.Claims.Add(new SelectedViewModel { Lable = claim, IsSelected = false });

            }
            return Ok(permissionRole);

        }
        [HttpPost("create-role-permission")]
        public async Task<IActionResult> AssignRolePermission(PermissionRoleViewModel permissionRole)
        {
            var role = await _roleManager.FindByIdAsync(permissionRole.RoleId);
            if (role is null) return NotFound("role is not found");
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            var transaction= _context.Database.BeginTransaction();
            try
            {
                foreach (var claim in roleClaims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                var selectedClaims = permissionRole.Claims.Where(x => x.IsSelected).Select(x=>x.Lable);
                foreach (var claim in selectedClaims)
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", claim));
                }
                await transaction.CommitAsync();
                return Ok(permissionRole);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound("role is not found"); }
            await _roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}
