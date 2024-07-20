using System.Security.Claims;

namespace FitnessApp.ApiGateway.Services.UserIdProvider;

public interface IUserIdProvider
{
    string GetUserId(ClaimsPrincipal user);
}