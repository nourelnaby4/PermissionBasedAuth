using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuth.Services;
using PermissionBasedAuth.ViewModels;

namespace PermissionBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) {
            _authService = authService;
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            var result = await _authService.SignInAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            var response = new
            {
                Token = result.Token,
                ExpiresOn = result.ExpiresOn
            };
            return Ok(response);
        }
    }
}
