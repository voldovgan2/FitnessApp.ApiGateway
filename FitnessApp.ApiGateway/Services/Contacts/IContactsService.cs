using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Contacts.Output;

namespace FitnessApp.ApiGateway.Services.Contacts;

public interface IContactsService
{
    Task<bool> CanViewUserContacts(GetUserContactsModel model);
    Task<IEnumerable<ContactModel>> GetUserContacts(GetUserContactsModel model);
    Task<UserContactsCountModel> GetUserContactsCount(string userId);
    Task<bool> IsFollower(GetFollowerStatusModel model);
    Task<IEnumerable<FollowerStatusModel>> IsFollowers(GetFollowersStatusModel model);
    Task<string> StartFollow(SendFollowModel model);
    Task<string> AcceptFollowRequest(ProcessFollowRequestModel model);
    Task<string> RejectFollowRequest(ProcessFollowRequestModel model);
    Task<string> DeleteFollowRequest(SendFollowModel model);
    Task<string> DeleteFollower(ProcessFollowRequestModel model);
    Task<string> UnfollowUser(SendFollowModel model);
}