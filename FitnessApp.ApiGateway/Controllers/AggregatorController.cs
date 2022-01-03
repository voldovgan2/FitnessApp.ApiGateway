using FitnessApp.ApiGateway.Contracts.Contacts.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Input;
using FitnessApp.ApiGateway.Contracts.Exercises.Output;
using FitnessApp.ApiGateway.Contracts.Food.Input;
using FitnessApp.ApiGateway.Contracts.Food.Output;
using FitnessApp.ApiGateway.Contracts.Settings.Input;
using FitnessApp.ApiGateway.Contracts.Settings.Output;
using FitnessApp.ApiGateway.Contracts.UserProfile.Input;
using FitnessApp.ApiGateway.Contracts.UserProfile.Output;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Settings.Input;
using FitnessApp.ApiGateway.Models.UserProfile.Input;
using FitnessApp.ApiGateway.Services.Aggregator;
using FitnessApp.ApiGateway.Services.UserIdProvider;
using FitnessApp.Paged.Contracts.Output;
using FitnessApp.Serializer.JsonMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [EnableCors("AllowAll")]
    [Authorize("Authenticated")]
    public class AggregatorController : Controller
    {
        private readonly IUserIdProvider _userIdProvider;
        private readonly IAggregatorService _aggregatorService;
        private readonly IJsonMapper _mapper;

        public AggregatorController
        (
            IUserIdProvider userIdProvider,
            IAggregatorService aggregatorService,
            IJsonMapper mapper
        )
        {
            _userIdProvider = userIdProvider;
            _aggregatorService = aggregatorService;
            _mapper = mapper;
        }

        #region Contacts

        [HttpGet("GetUserContacts")]
        public async Task<IActionResult> GetUserContactsAsync([FromQuery]GetUserContactsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<GetUserContactsModel>(contract);
            model.UserId = userId;
            if(model.ContactsUserId == null)
            {
                model.ContactsUserId = userId;
            }
            var canViewUserContacts = await _aggregatorService.CanViewUserContactsAsync(model);
            if (canViewUserContacts)
            {
                var response = await _aggregatorService.GetUserContactsAsync(model);
                if (response != null)
                {
                    var result = _mapper.Convert<PagedDataContract<UsersProfilesShortContract>>(response);
                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost("StartFollow")]
        public async Task<IActionResult> StartFollowAsync([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.StartFollowAsync(model);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("AcceptFollowRequest")]
        public async Task<IActionResult> AcceptFollowRequestAsync([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.AcceptFollowRequestAsync(model);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("RejectFollowRequest")]
        public async Task<IActionResult> RejectFollowRequestAsync([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.RejectFollowRequestAsync(model);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("DeleteFollowRequest")]
        public async Task<IActionResult> DeleteFollowRequestAsync([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.DeleteFollowRequestAsync(model);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("DeleteFollower")]
        public async Task<IActionResult> DeleteFollowerAsync([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.DeleteFollowerAsync(model);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("UnfollowUser")]
        public async Task<IActionResult> UnfollowUserAsync([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<SendFollowModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.UnfollowUserAsync(model);
            if (updated != null)
            {
                return Ok(updated);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Settings

        [HttpGet("GetSettings")]
        public async Task<IActionResult> GetSettingsAsync()
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.GetSettingsAsync(userId);
            if (response != null)
            {
                var result = _mapper.Convert<SettingsContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("CreateSettings")]
        public async Task<IActionResult> CreateSettingsAsync([FromBody]CreateSettingsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<CreateSettingsModel>(contract);
            model.UserId = userId;
            var created = await _aggregatorService.CreateSettingsAsync(model);
            if (created != null)
            {
                var result = _mapper.Convert<SettingsContract>(created);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("UpdateSettings")]
        public async Task<IActionResult> UpdateSettingsAsync([FromBody]UpdateSettingsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<UpdateSettingsModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.UpdateSettingsAsync(model);
            if (updated != null)
            {
                var result = _mapper.Convert<SettingsContract>(updated);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteSettings")]
        public async Task<IActionResult> DeleteSettingsAsync()
        {
            var userId = _userIdProvider.GetUserId(User);
            var deleted = await _aggregatorService.DeleteSettingsAsync(userId);
            if (deleted != null)
            {
                return Ok(deleted);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region UserProfile
        
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var userId = _userIdProvider.GetUserId(User);
            var result = await GetUserProfileByIdAsync(new GetUserProfileModel 
            {
                UserId = userId,
                ContactsUserId = userId
            });
            return result;
        }

        [HttpGet("GetUserProfile/{userId}")]
        public async Task<IActionResult> GetUserProfileAsync([FromRoute] string userId)
        {
            var currentUserId = _userIdProvider.GetUserId(User);
            var result = await GetUserProfileByIdAsync(new GetUserProfileModel 
            {
                UserId = currentUserId,
                ContactsUserId = userId
            });
            return result;
        }

        [HttpPost("CreateUserProfile")]
        public async Task<IActionResult> CreateUserProfileAsync([FromBody]CreateUserProfileContract contract)
        {
            var model = _mapper.Convert<CreateUserProfileModel>(contract);
            var created = await _aggregatorService.CreateUserProfileAsync(model);
            if (created != null)
            {
                var result = _mapper.Convert<UserProfileContract>(created);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody]UpdateUserProfileContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<UpdateUserProfileModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.UpdateUserProfileAsync(model);
            if (updated != null)
            {
                var result = _mapper.Convert<UserProfileContract>(updated);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteUserProfile/{userId}")]
        public async Task<IActionResult> DeleteUserProfileAsync([FromRoute] string userId)
        {
            var deleted = await _aggregatorService.DeleteUserProfileAsync(userId);
            if (deleted != null)
            {
                return Ok(deleted);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        private async Task<IActionResult> GetUserProfileByIdAsync(GetUserProfileModel model)
        {
            var response = await _aggregatorService.GetUserProfileAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<UserProfileContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Food

        [HttpGet("GetFood")]
        public async Task<IActionResult> GetFoodAsync([FromQuery] GetUserFoodsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<GetUserFoodsModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.GetFoodsAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<UserFoodsContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("AddFood")]
        public async Task<IActionResult> AddFoodAsync([FromBody] AddUserFoodContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<AddUserFoodModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.AddFoodAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<FoodItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("EditFood")]
        public async Task<IActionResult> EditFoodAsync([FromBody] UpdateUserFoodContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<UpdateUserFoodModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.EditFoodAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<FoodItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("RemoveFood/{foodId}")]
        public async Task<IActionResult> RemoveFoodAsync([FromRoute] string foodId)
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.RemoveFoodAsync(userId, foodId);
            if (response != null)
            {
                var result = _mapper.Convert<string>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Exercises

        [HttpGet("GetExercises")]
        public async Task<IActionResult> GetExercisesAsync([FromQuery] GetUserExercisesContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<GetUserExercisesModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.GetExercisesAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<UserExercisesContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("AddExercise")]
        public async Task<IActionResult> AddExerciseAsync([FromBody] AddUserExerciseContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<AddUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.AddExerciseAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<ExerciseItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("EditExercise")]
        public async Task<IActionResult> EditExerciseAsync([FromBody] UpdateUserExerciseContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Convert<UpdateUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.EditExerciseAsync(model);
            if (response != null)
            {
                var result = _mapper.Convert<ExerciseItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseAsync([FromRoute] string exerciseId)
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.RemoveExerciseAsync(userId, exerciseId);
            if (response != null)
            {
                var result = _mapper.Convert<string>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }
}