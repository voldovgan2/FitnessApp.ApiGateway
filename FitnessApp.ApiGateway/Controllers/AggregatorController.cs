using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
using FitnessApp.Common.Paged.Contracts.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace FitnessApp.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [EnableCors("AllowAll")]

    // [Authorize("Authenticated")]
    // [RequiredScope(ScopeRequiredByApi)]
    public class AggregatorController : Controller
    {
#pragma warning disable S1144 // Unused private types or members should be removed
        private const string ScopeRequiredByApi = "User.Read";
#pragma warning restore S1144 // Unused private types or members should be removed

        private readonly IUserIdProvider _userIdProvider;
        private readonly IAggregatorService _aggregatorService;
        private readonly IMapper _mapper;

        public AggregatorController(
            IUserIdProvider userIdProvider,
            IAggregatorService aggregatorService,

            IMapper mapper
        )
        {
            _userIdProvider = userIdProvider;
            _aggregatorService = aggregatorService;
            _mapper = mapper;
        }

        #region Test

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
#pragma warning disable S1075 // URIs should not be hardcoded
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, "http://localhost:7071/api/FollowRequestConfirmed");
#pragma warning restore S1075 // URIs should not be hardcoded
            var payload = new
            {
                Sender = "user0",
                Receiver = "volodimir.dovgan@hotmail.com",
                MessageType = "StartFollow"
            };
            request.Content = new System.Net.Http.StringContent(
                new Common.Serializer.JsonSerializer.JsonSerializer().SerializeToString(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // request.Headers.Add("Authorization", $"Bearer {token}");
            var internalHttpClient = new System.Net.Http.HttpClient();

            var response = await internalHttpClient.SendAsync(request);
#pragma warning disable S1481 // Unused local variables should be removed
            var content = await response.Content.ReadAsStringAsync();
#pragma warning restore S1481 // Unused local variables should be removed

            // var response = await _aggregatorService.GetSettings("1");
            return Ok("Test");
        }

        [HttpGet]
        public Task<System.Collections.Generic.List<TodoItem>> GetTodoItems()
        {
            return Task.FromResult(new System.Collections.Generic.List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Description = "Description",
                    Owner = "Owner",
                    Status = true
                }
            });
        }

        [HttpGet("{id}")]
        public Task<TodoItem> GetTodoItem(int id)
        {
            return Task.FromResult(new TodoItem
            {
                Id = id,
                Description = "Description",
                Owner = "Owner",
                Status = true
            });
        }

        #endregion

        #region Internal Token

        [HttpGet("InternalToken")]
        public async Task<ActionResult<string>> GetInternalToken()
        {
            var token = await _aggregatorService.GetToken();
            return token;
        }

        #endregion

        #region Contacts

        [HttpGet("GetUserContacts")]
        public async Task<IActionResult> GetUserContacts([FromQuery]GetUserContactsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<GetUserContactsModel>(contract);
            model.UserId = userId;
            model.ContactsUserId ??= userId;

            var canViewUserContacts = await _aggregatorService.CanViewUserContacts(model);
            if (canViewUserContacts)
            {
                var response = await _aggregatorService.GetUserContacts(model);
                if (response != null)
                {
                    var result = _mapper.Map<PagedDataContract<UsersProfilesShortContract>>(response);
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
        public async Task<IActionResult> StartFollow([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.StartFollow(model);
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
        public async Task<IActionResult> AcceptFollowRequest([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.AcceptFollowRequest(model);
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
        public async Task<IActionResult> RejectFollowRequest([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.RejectFollowRequest(model);
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
        public async Task<IActionResult> DeleteFollowRequest([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.DeleteFollowRequest(model);
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
        public async Task<IActionResult> DeleteFollower([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.DeleteFollower(model);
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
        public async Task<IActionResult> UnfollowUser([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.UnfollowUser(model);
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
        public async Task<IActionResult> GetSettings()
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.GetSettings(userId);
            if (response != null)
            {
                var result = _mapper.Map<SettingsContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("CreateSettings")]
        public async Task<IActionResult> CreateSettings([FromBody]CreateSettingsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<CreateSettingsModel>(contract);
            model.UserId = userId;
            var created = await _aggregatorService.CreateSettings(model);
            if (created != null)
            {
                var result = _mapper.Map<SettingsContract>(created);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("UpdateSettings")]
        public async Task<IActionResult> UpdateSettings([FromBody]UpdateSettingsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateSettingsModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.UpdateSettings(model);
            if (updated != null)
            {
                var result = _mapper.Map<SettingsContract>(updated);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteSettings")]
        public async Task<IActionResult> DeleteSettings()
        {
            var userId = _userIdProvider.GetUserId(User);
            var deleted = await _aggregatorService.DeleteSettings(userId);
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
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = _userIdProvider.GetUserId(User);
            var result = await GetUserProfileById(new GetUserProfileModel
            {
                UserId = userId,
                ContactsUserId = userId
            });
            return result;
        }

        [HttpGet("GetUserProfile/{userId}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] string userId)
        {
            var currentUserId = _userIdProvider.GetUserId(User);
            var result = await GetUserProfileById(new GetUserProfileModel
            {
                UserId = currentUserId,
                ContactsUserId = userId
            });
            return result;
        }

        [HttpPost("CreateUserProfile")]
        public async Task<IActionResult> CreateUserProfile([FromBody]CreateUserProfileContract contract)
        {
            var model = _mapper.Map<CreateUserProfileModel>(contract);
            var created = await _aggregatorService.CreateUserProfile(model);
            if (created != null)
            {
                var result = _mapper.Map<UserProfileContract>(created);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody]UpdateUserProfileContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateUserProfileModel>(contract);
            model.UserId = userId;
            var updated = await _aggregatorService.UpdateUserProfile(model);
            if (updated != null)
            {
                var result = _mapper.Map<UserProfileContract>(updated);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("DeleteUserProfile/{userId}")]
        public async Task<IActionResult> DeleteUserProfile([FromRoute] string userId)
        {
            var deleted = await _aggregatorService.DeleteUserProfile(userId);
            if (deleted != null)
            {
                return Ok(deleted);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        private async Task<IActionResult> GetUserProfileById(GetUserProfileModel model)
        {
            var response = await _aggregatorService.GetUserProfile(model);
            if (response != null)
            {
                var result = _mapper.Map<UserProfileContract>(response);
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
        public async Task<IActionResult> GetFood([FromQuery] GetUserFoodsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<GetUserFoodsModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.GetFoods(model);
            if (response != null)
            {
                var result = _mapper.Map<UserFoodsContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("AddFood")]
        public async Task<IActionResult> AddFood([FromBody] AddUserFoodContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<AddUserFoodModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.AddFood(model);
            if (response != null)
            {
                var result = _mapper.Map<FoodItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("EditFood")]
        public async Task<IActionResult> EditFood([FromBody] UpdateUserFoodContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateUserFoodModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.EditFood(model);
            if (response != null)
            {
                var result = _mapper.Map<FoodItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("RemoveFood/{foodId}")]
        public async Task<IActionResult> RemoveFood([FromRoute] string foodId)
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.RemoveFood(userId, foodId);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Exercises

        [HttpGet("GetExercises")]
        public async Task<IActionResult> GetExercises([FromQuery] GetUserExercisesContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<GetUserExercisesModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.GetExercises(model);
            if (response != null)
            {
                var result = _mapper.Map<UserExercisesContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("AddExercise")]
        public async Task<IActionResult> AddExercise([FromBody] AddUserExerciseContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<AddUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.AddExercise(model);
            if (response != null)
            {
                var result = _mapper.Map<ExerciseItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("EditExercise")]
        public async Task<IActionResult> EditExercise([FromBody] UpdateUserExerciseContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.EditExercise(model);
            if (response != null)
            {
                var result = _mapper.Map<ExerciseItemContract>(response);
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<IActionResult> RemoveExercise([FromRoute] string exerciseId)
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.RemoveExercise(userId, exerciseId);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public bool Status { get; set; }
    }
}