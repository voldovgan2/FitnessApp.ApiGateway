using System.Collections.Generic;
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
using FitnessApp.ApiGateway.Exceptions;
using FitnessApp.ApiGateway.Models.Contacts.Input;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Notification;
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
        public async Task Test()
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
        }

        [HttpGet]
        public Task<List<TodoItem>> GetTodoItems()
        {
            return Task.FromResult(new List<TodoItem>
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

        #region Contacts

        [HttpGet("GetUserContacts")]
        public async Task<PagedDataContract<UsersProfilesShortContract>> GetUserContacts([FromQuery]GetUserContactsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<GetUserContactsModel>(contract);
            model.UserId = userId;
            model.ContactsUserId ??= userId;

            var canViewUserContacts = await _aggregatorService.CanViewUserContacts(model);
            if (canViewUserContacts)
            {
                var response = await _aggregatorService.GetUserContacts(model);
                return _mapper.Map<PagedDataContract<UsersProfilesShortContract>>(response);
            }
            else
            {
                throw new ForbidenException("Access denied to this resource");
            }
        }

        [HttpPost("StartFollow")]
        public async Task<string> StartFollow([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.StartFollow(model);
            return result;
        }

        [HttpPost("AcceptFollowRequest")]
        public async Task<string> AcceptFollowRequest([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.AcceptFollowRequest(model);
            return result;
        }

        [HttpPost("RejectFollowRequest")]
        public async Task<string> RejectFollowRequest([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.RejectFollowRequest(model);
            return result;
        }

        [HttpPost("DeleteFollowRequest")]
        public async Task<string> DeleteFollowRequest([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.DeleteFollowRequest(model);
            return result;
        }

        [HttpPost("DeleteFollower")]
        public async Task<string> DeleteFollower([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.DeleteFollower(model);
            return result;
        }

        [HttpPost("UnfollowUser")]
        public async Task<string> UnfollowUser([FromBody] SendFollowContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await _aggregatorService.UnfollowUser(model);
            return result;
        }

        #endregion

        #region Settings

        [HttpGet("GetSettings")]
        public async Task<SettingsContract> GetSettings()
        {
            var userId = "test".Length == 0 ?
                _userIdProvider.GetUserId(User)
                : "savaTest";
            var response = await _aggregatorService.GetSettings(userId);
            return _mapper.Map<SettingsContract>(response);
        }

        [HttpPost("CreateSettings")]
        public async Task<SettingsContract> CreateSettings([FromBody]CreateSettingsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<CreateSettingsModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.CreateSettings(model);
            return _mapper.Map<SettingsContract>(response);
        }

        [HttpPut("UpdateSettings")]
        public async Task<SettingsContract> UpdateSettings([FromBody]UpdateSettingsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateSettingsModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.UpdateSettings(model);
            return _mapper.Map<SettingsContract>(response);
        }

        [HttpDelete("DeleteSettings")]
        public async Task<string> DeleteSettings()
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.DeleteSettings(userId);
            return response;
        }

        #endregion

        #region UserProfile

        [HttpGet("GetUserProfile")]
        public async Task<UserProfileContract> GetUserProfile()
        {
            var currentUserId = _userIdProvider.GetUserId(User);
            return await GetUserProfileById(new GetUserProfileModel
            {
                UserId = currentUserId,
                ContactsUserId = currentUserId
            });
        }

        [HttpGet("GetUserProfile/{userId}")]
        public async Task<UserProfileContract> GetUserProfile([FromRoute] string userId)
        {
            var currentUserId = _userIdProvider.GetUserId(User);
            return await GetUserProfileById(new GetUserProfileModel
            {
                UserId = currentUserId,
                ContactsUserId = userId
            });
        }

        [HttpPost("CreateUserProfile")]
        public async Task<UserProfileContract> CreateUserProfile([FromBody]CreateUserProfileContract contract)
        {
            var model = _mapper.Map<CreateUserProfileModel>(contract);
            var response = await _aggregatorService.CreateUserProfile(model);
            return _mapper.Map<UserProfileContract>(response);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<UserProfileContract> UpdateUserProfile([FromBody]UpdateUserProfileContract contract)
        {
            var currentUserId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateUserProfileModel>(contract);
            model.UserId = currentUserId;
            var response = await _aggregatorService.UpdateUserProfile(model);
            return _mapper.Map<UserProfileContract>(response);
        }

        [HttpDelete("DeleteUserProfile")]
        public async Task<string> DeleteUserProfile()
        {
            var currentUserId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.DeleteUserProfile(currentUserId);
            return response;
        }

        private async Task<UserProfileContract> GetUserProfileById(GetUserProfileModel model)
        {
            var response = await _aggregatorService.GetUserProfile(model);
            return _mapper.Map<UserProfileContract>(response);
        }

        #endregion

        #region Food

        [HttpGet("GetFood")]
        public async Task<UserFoodsContract> GetFood([FromQuery] GetUserFoodsContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<GetUserFoodsModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.GetFoods(model);
            return _mapper.Map<UserFoodsContract>(response);
        }

        [HttpPut("AddFood")]
        public async Task<FoodItemContract> AddFood([FromBody] AddUserFoodContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<AddUserFoodModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.AddFood(model);
            return _mapper.Map<FoodItemContract>(response);
        }

        [HttpPut("EditFood")]
        public async Task<FoodItemContract> EditFood([FromBody] UpdateUserFoodContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateUserFoodModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.EditFood(model);
            return _mapper.Map<FoodItemContract>(response);
        }

        [HttpDelete("RemoveFood/{foodId}")]
        public async Task<string> RemoveFood([FromRoute] string foodId)
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.RemoveFood(userId, foodId);
            return _mapper.Map<string>(response);
        }

        #endregion

        #region Exercises

        [HttpGet("GetExercises")]
        public async Task<UserExercisesContract> GetExercises([FromQuery] GetUserExercisesContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<GetUserExercisesModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.GetExercises(model);
            return _mapper.Map<UserExercisesContract>(response);
        }

        [HttpPut("AddExercise")]
        public async Task<ExerciseItemContract> AddExercise([FromBody] AddUserExerciseContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<AddUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.AddExercise(model);
            return _mapper.Map<ExerciseItemContract>(response);
        }

        [HttpPut("EditExercise")]
        public async Task<ExerciseItemContract> EditExercise([FromBody] UpdateUserExerciseContract contract)
        {
            var userId = _userIdProvider.GetUserId(User);
            var model = _mapper.Map<UpdateUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await _aggregatorService.EditExercise(model);
            return _mapper.Map<ExerciseItemContract>(response);
        }

        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<string> RemoveExercise([FromRoute] string exerciseId)
        {
            var userId = _userIdProvider.GetUserId(User);
            var response = await _aggregatorService.RemoveExercise(userId, exerciseId);
            return response;
        }

        #endregion

        #region Notification

        [HttpGet("GetNotificationTicket")]
        public Task<string> GetNotificationTicket()
        {
            var model = new NotificationTicketModel
            {
                Ip = GetRequestIp(),
                UserId = _userIdProvider.GetUserId(User),
            };
            return _aggregatorService.GetNotificationTicket(model);
        }

        [HttpGet("ValidateNotificationTicket")]
        public Task<bool> ValidateNotificationTicket([FromQuery] string ticket)
        {
            var model = new ValidateNotificationTicketModel
            {
                Ticket = ticket,
                Ip = GetRequestIp(),
                UserId = _userIdProvider.GetUserId(User)
            };
            return _aggregatorService.ValidateNotificationTicket(model);
        }

        private string GetRequestIp()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
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