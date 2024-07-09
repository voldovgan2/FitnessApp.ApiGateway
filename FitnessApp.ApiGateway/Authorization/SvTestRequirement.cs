using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.ApiGateway.Authorization
{
    public class SvTestRequirement : IAuthorizationRequirement;

    public class SvTestRequirementHandler : AuthorizationHandler<SvTestRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SvTestRequirement requirement)
        {
            var permission = context.User.FindFirst(c => c.Type == "Permission");
            if (permission?.Value == "ToMultiply")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
