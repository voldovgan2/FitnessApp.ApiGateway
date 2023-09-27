using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;

namespace FitnessApp.ApiGateway.Services.Settings
{
    public interface ISettingsService
    {
        Task<SettingsModel> GetSettings(string userId);
        Task<SettingsModel> CreateSettings(CreateSettingsModel model);
        Task<SettingsModel> UpdateSettings(UpdateSettingsModel model);
        Task<string> DeleteSettings(string userId);
    }
}