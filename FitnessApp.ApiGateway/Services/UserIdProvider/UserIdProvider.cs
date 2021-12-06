using System.Linq;
using System.Security.Claims;

namespace FitnessApp.ApiGateway.Services.UserIdProvider
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(ClaimsPrincipal user)
        {
            return (user.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}