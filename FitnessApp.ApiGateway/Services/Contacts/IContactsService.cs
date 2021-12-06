using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Contacts.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Contacts
{
    public interface IContactsService
    {
        Task<bool> CanViewUserContactsAsync(GetUserContactsModel model);
        Task<IEnumerable<ContactModel>> GetUserContactsAsync(GetUserContactsModel model);
        Task<UserContactsCountModel> GetUserContactsCountAsync(string userId);
        Task<bool> IsFollowerAsync(GetUserContactsModel model);
        Task<string> StartFollowAsync(SendFollowModel model);
        Task<string> AcceptFollowRequestAsync(ProcessFollowRequestModel model);
        Task<string> RejectFollowRequestAsync(ProcessFollowRequestModel model);
        Task<string> DeleteFollowRequestAsync(SendFollowModel model);
        Task<string> DeleteFollowerAsync(ProcessFollowRequestModel model);
        Task<string> UnfollowUserAsync(SendFollowModel model);
    }
}