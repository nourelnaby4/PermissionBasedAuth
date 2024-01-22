using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PermissionBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleManager.Roles.Select(x => new { x.Id, x.Name }).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if(await _roleManager.RoleExistsAsync(roleName)) { return BadRequest("role is existed alread"); }
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return Ok(roleName);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role =await _roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound("role is not found"); }
            await _roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}
