using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using PermissionBasedAuth.Constants;
using PermissionBasedAuth.ViewModels;

namespace PermissionBasedAuth.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private JWT _jwt;
        public PermissionAuthorizationHandler(IOptions<JWT> jwt)
        {
            _jwt = jwt.Value;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;

            var canAccess = context.User.Claims.Any(c => c.Type==nameof(ClaimType.Permission) && c.Value == requirement.Permission && c.Issuer== _jwt.Issuer);

            if (canAccess)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
