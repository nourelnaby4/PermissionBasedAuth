using Microsoft.AspNetCore.Authorization;
using PermissionBasedAuth.Constants;

namespace PermissionBasedAuth.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {

        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User is null)
                return;

            var canAccess = context.User.Claims.Any(x => x.Type == nameof(ClaimType.Permission) && x.Value == requirement.Permission && x.Issuer == "LOCAL AUTHORITY");
            if (canAccess)
            {
                context.Succeed(requirement); 
                return;
            }
        }
    }
}
