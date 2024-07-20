using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Services.Abstractions;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Settings;

public class SettingsService(ApiClientSettings apiClientSettings, IInternalClient internalClient) :
    GenericService<SettingsModel>(apiClientSettings, internalClient),  ISettingsService
{
    private const string API = "Settings";
    private const string GET_SETTINGS_METHOD = "GetSettings";
    private const string CREATE_SETTINGS_METHOD = "CreateSettings";
    private const string UPDATE_SETTINGS_METHOD = "UpdateSettings";
    private const string DELETE_SETTINGS_METHOD = "DeleteSettings";

    public Task<SettingsModel> GetSettings(string userId)
    {
        return GetItem(ApiClientSettings.Url, API, GET_SETTINGS_METHOD, userId);
    }

    public Task<SettingsModel> CreateSettings(CreateSettingsModel model)
    {
        return CreateItem(ApiClientSettings.Url, API, CREATE_SETTINGS_METHOD, model);
    }

    public Task<SettingsModel> UpdateSettings(UpdateSettingsModel model)
    {
        return UpdateItem(ApiClientSettings.Url, API, UPDATE_SETTINGS_METHOD, model);
    }

    public Task<string> DeleteSettings(string userId)
    {
        return DeleteItem(ApiClientSettings.Url, API, DELETE_SETTINGS_METHOD, userId);
    }
}