using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Services.Abstractions.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.UserProfile
{
    public interface IUserProfileService<Model> : IGenericService<Model>
    { 
        Task<IEnumerable<Model>> GetUsersProfilesAsync(GetSelectedUsersProfilesModel model);
    }
}