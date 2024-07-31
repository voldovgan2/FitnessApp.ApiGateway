using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Enums.Contacts;
using FitnessApp.ApiGateway.Enums.Settings;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Contacts.Output;
using FitnessApp.ApiGateway.Models.Internal;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.Settings;

namespace FitnessApp.ApiGateway.Services.Contacts;

public class ContactsService(
    ApiClientSettings apiClientSettings,
    ISettingsService settingsService,
    IInternalClient internalClient) : IContactsService
{
    private const string GET_USER_CONTACTS_METHOD = "GetUserContacts";
    private const string GET_USER_CONTACTS_COUNT_METHOD = "GetUserContactsCount";
    private const string GET_IS_FOLLOWER_METHOD = "GetIsFollower";
    private const string GET_IS_FOLLOWERS_METHOD = "GetIsFollowers";
    private const string START_FOLLOW_METHOD = "StartFollow";
    private const string ACCEPT_FOLLOW_REQUEST_METHOD = "AcceptFollowRequest";
    private const string REJECT_FOLLOW_REQUEST_METHOD = "RejectFollowRequest";
    private const string DELETE_FOLLOW_REQUEST_METHOD = "DeleteFollowRequest";
    private const string DELETE_FOLLOWER_METHOD = "DeleteFollower";
    private const string UNFOLLOW_USER_METHOD = "UnfollowUser";

    public async Task<bool> CanViewUserContacts(GetUserContactsModel model)
    {
        var result = model.ContactsUserId == model.UserId;
        if (!result)
        {
            var settings = await settingsService.GetSettings(model.ContactsUserId);
            if (settings != null)
            {
                var getFollowerStatusModel = new GetFollowerStatusModel
                {
                    UserId = model.UserId,
                    ContactsUserId = model.ContactsUserId,
                };
                result =
                    (model.ContactsType == ContactsType.Followers
                        && ((settings.CanViewFollowers == PrivacyType.All) || (settings.CanViewFollowers == PrivacyType.Followers && await IsFollower(getFollowerStatusModel))))
                    || (model.ContactsType == ContactsType.Followings
                        && ((settings.CanViewFollowings == PrivacyType.All) || (settings.CanViewFollowings == PrivacyType.Followers && await IsFollower(getFollowerStatusModel))));
            }
        }

        return result;
    }

    public async Task<IEnumerable<ContactModel>> GetUserContacts(GetUserContactsModel model)
    {
        var payload = new
        {
            UserId = model.ContactsUserId,
            model.ContactsType
        };
        var request = new InternalRequest(
            HttpMethod.Get,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            GET_USER_CONTACTS_METHOD,
            null,
            null,
            payload);
        var result = await internalClient.SendInternalRequest<List<ContactModel>>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<UserContactsCountModel> GetUserContactsCount(string userId)
    {
        var request = new InternalRequest(
            HttpMethod.Get,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            GET_USER_CONTACTS_COUNT_METHOD,
            [userId],
            null,
            null);
        var result = await internalClient.SendInternalRequest<UserContactsCountModel>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<bool> IsFollower(GetFollowerStatusModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Get,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            GET_IS_FOLLOWER_METHOD,
            null,
            null,
            model);
        var result = await internalClient.SendInternalRequest<bool>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<IEnumerable<FollowerStatusModel>> IsFollowers(GetFollowersStatusModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            GET_IS_FOLLOWERS_METHOD,
            null,
            model,
            null);
        var result = await internalClient.SendInternalRequest<IEnumerable<FollowerStatusModel>>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<string> StartFollow(SendFollowModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            START_FOLLOW_METHOD,
            null,
            model,
            null);
        var result = await internalClient.SendInternalRequest<string>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<string> AcceptFollowRequest(ProcessFollowRequestModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            ACCEPT_FOLLOW_REQUEST_METHOD,
            null,
            model,
            null);
        var result = await internalClient.SendInternalRequest<string>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<string> RejectFollowRequest(ProcessFollowRequestModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            REJECT_FOLLOW_REQUEST_METHOD,
            null,
            model,
            null);
        var result = await internalClient.SendInternalRequest<string>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<string> DeleteFollowRequest(SendFollowModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            DELETE_FOLLOW_REQUEST_METHOD,
            null,
            model,
            null);
        var result = await internalClient.SendInternalRequest<string>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<string> DeleteFollower(ProcessFollowRequestModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            DELETE_FOLLOWER_METHOD,
            null,
            model,
            null
        );
        var result = await internalClient.SendInternalRequest<string>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }

    public async Task<string> UnfollowUser(SendFollowModel model)
    {
        var request = new InternalRequest(
            HttpMethod.Post,
            apiClientSettings.Url,
            apiClientSettings.ApiName,
            UNFOLLOW_USER_METHOD,
            null,
            model,
            null
        );
        var result = await internalClient.SendInternalRequest<string>(
            apiClientSettings.ApiName,
            apiClientSettings.Scope,
            request);
        return result;
    }
}