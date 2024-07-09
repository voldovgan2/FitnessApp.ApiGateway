using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FitnessApp.ApiGateway.Configuration
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication2(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.Authority = configuration["ClientAuthenticationSettings:Issuer"];
                    cfg.Audience = configuration["ClientAuthenticationSettings:Audience"];
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = configuration["ClientAuthenticationSettings:Audience"],
                        ValidIssuer = configuration["ClientAuthenticationSettings:Issuer"]
                    };
                });
            return services;
        }
    }
}
