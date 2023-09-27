using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Services.Contacts;
using FitnessApp.ApiGateway.Services.Exercises;
using FitnessApp.ApiGateway.Services.Food;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.ApiGateway.Services.SignalR;
using FitnessApp.ApiGateway.Services.TokenClient;
using FitnessApp.ApiGateway.Services.UserProfile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessApp.ApiGateway.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddSettingsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISettingsService, SettingsService>(
                sp =>
                {
                    return new SettingsService(
                        GetApiClientSettings("Settings", configuration),
                        sp.GetRequiredService<IInternalClient>());
                }
            );
            return services;
        }

        public static IServiceCollection AddUserProfileService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserProfileService, UserProfileService>(
                sp =>
                {
                    return new UserProfileService(
                        GetApiClientSettings("UserProfile", configuration),
                        sp.GetRequiredService<IInternalClient>());
                }
            );
            return services;
        }

        public static IServiceCollection AddContactsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContactsService, ContactsService>(
                sp =>
                {
                    return new ContactsService(
                        GetApiClientSettings("Contacts", configuration),
                        sp.GetRequiredService<ISettingsService>(),
                        sp.GetRequiredService<IInternalClient>());
                }
            );
            return services;
        }

        public static IServiceCollection AddFoodService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IFoodService, FoodService>(
                sp =>
                {
                    return new FoodService(
                        GetApiClientSettings("Food", configuration),
                        sp.GetRequiredService<IInternalClient>());
                }
            );
            return services;
        }

        public static IServiceCollection AddExercisesService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IExercisesService, ExercisesService>(
                sp =>
                {
                    return new ExercisesService(
                        GetApiClientSettings("Exercises", configuration),
                        sp.GetRequiredService<IInternalClient>());
                }
            );
            return services;
        }

        public static IServiceCollection AddSignalRService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISignalR, SignalR>(
                sp =>
                {
                    return new SignalR(
                        GetApiClientSettings("SignalR", configuration),
                        sp.GetRequiredService<ITokenClient>(),
                        sp.GetRequiredService<IInternalClient>());
                }
            );
            return services;
        }

        private static ApiClientSettings GetApiClientSettings(string apiName, IConfiguration configuration)
        {
            var apiClientSettingsSection = configuration.GetSection($"Apis:{apiName}");
            var apiClientSettings = new ApiClientSettings
            {
                ApiName = apiName
            };
            apiClientSettingsSection.Bind(apiClientSettings);
            return apiClientSettings;
        }
    }
}