using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

namespace FitnessApp.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [EnableCors("AllowAll")]

    [Authorize]
    public class AggregatorController(
        IUserIdProvider userIdProvider,
        IAggregatorService aggregatorService,
        IMapper mapper) : Controller
    {
        #region Test

        [HttpGet("Test")]
        public async Task<SettingsContract> TestGet()
        {
            var userId = "test".Length == 0 ?
                            userIdProvider.GetUserId(User)
                            : "savaTest";
            var response = await aggregatorService.GetSettings(userId);
            return mapper.Map<SettingsContract>(response);
        }

        [HttpGet]
        public Task<List<TodoItem>> GetTodoItems()
        {
            return Task.FromResult(new List<TodoItem>
            {
                new()
                {
                    Id = 1,
                    Description = "Description",
                    Owner = "Owner",
                    Status = true
                }
            });
        }

        [HttpPost]
        public async Task<SettingsContract> TestPost([FromBody] CreateSettingsContract contract)
        {
            var userId = "test".Length == 0 ?
                userIdProvider.GetUserId(User)
                : "savaTest";
            var model = mapper.Map<CreateSettingsModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.CreateSettings(model);
            return mapper.Map<SettingsContract>(response);
        }

        #endregion

        #region Contacts

        [HttpGet("GetUserContacts")]
        public async Task<PagedDataContract<UsersProfilesShortContract>> GetUserContacts([FromQuery]GetUserContactsContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<GetUserContactsModel>(contract);
            model.UserId = userId;
            model.ContactsUserId ??= userId;

            var canViewUserContacts = await aggregatorService.CanViewUserContacts(model);
            if (canViewUserContacts)
            {
                var response = await aggregatorService.GetUserContacts(model);
                return mapper.Map<PagedDataContract<UsersProfilesShortContract>>(response);
            }
            else
            {
                throw new ForbidenException("Access denied to this resource");
            }
        }

        [HttpPost("StartFollow")]
        public async Task<string> StartFollow([FromBody] SendFollowContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await aggregatorService.StartFollow(model);
            return result;
        }

        [HttpPost("AcceptFollowRequest")]
        public async Task<string> AcceptFollowRequest([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var result = await aggregatorService.AcceptFollowRequest(model);
            return result;
        }

        [HttpPost("RejectFollowRequest")]
        public async Task<string> RejectFollowRequest([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var result = await aggregatorService.RejectFollowRequest(model);
            return result;
        }

        [HttpPost("DeleteFollowRequest")]
        public async Task<string> DeleteFollowRequest([FromBody] SendFollowContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await aggregatorService.DeleteFollowRequest(model);
            return result;
        }

        [HttpPost("DeleteFollower")]
        public async Task<string> DeleteFollower([FromBody] ProcessFollowRequestContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<ProcessFollowRequestModel>(contract);
            model.UserId = userId;
            var result = await aggregatorService.DeleteFollower(model);
            return result;
        }

        [HttpPost("UnfollowUser")]
        public async Task<string> UnfollowUser([FromBody] SendFollowContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<SendFollowModel>(contract);
            model.UserId = userId;
            var result = await aggregatorService.UnfollowUser(model);
            return result;
        }

        #endregion

        #region Settings

        [HttpGet("GetSettings")]
        public async Task<SettingsContract> GetSettings()
        {
            var userId = userIdProvider.GetUserId(User);
            var response = await aggregatorService.GetSettings(userId);
            return mapper.Map<SettingsContract>(response);
        }

        [HttpPost("CreateSettings")]
        public async Task<SettingsContract> CreateSettings([FromBody]CreateSettingsContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<CreateSettingsModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.CreateSettings(model);
            return mapper.Map<SettingsContract>(response);
        }

        [HttpPut("UpdateSettings")]
        public async Task<SettingsContract> UpdateSettings([FromBody]UpdateSettingsContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<UpdateSettingsModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.UpdateSettings(model);
            return mapper.Map<SettingsContract>(response);
        }

        [HttpDelete("DeleteSettings")]
        public async Task<string> DeleteSettings()
        {
            var userId = userIdProvider.GetUserId(User);
            var response = await aggregatorService.DeleteSettings(userId);
            return response;
        }

        #endregion

        #region UserProfile

        [HttpGet("GetUserProfile")]
        public async Task<UserProfileContract> GetUserProfile()
        {
            var currentUserId = userIdProvider.GetUserId(User);
            return await GetUserProfileById(new GetUserProfileModel
            {
                UserId = currentUserId,
                ContactsUserId = currentUserId
            });
        }

        [HttpGet("GetUserProfile/{userId}")]
        public async Task<UserProfileContract> GetUserProfile([FromRoute] string userId)
        {
            var currentUserId = userIdProvider.GetUserId(User);
            return await GetUserProfileById(new GetUserProfileModel
            {
                UserId = currentUserId,
                ContactsUserId = userId
            });
        }

        [HttpPost("CreateUserProfile")]
        public async Task<UserProfileContract> CreateUserProfile([FromBody]CreateUserProfileContract contract)
        {
            var model = mapper.Map<CreateUserProfileModel>(contract);
            var response = await aggregatorService.CreateUserProfile(model);
            return mapper.Map<UserProfileContract>(response);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<UserProfileContract> UpdateUserProfile([FromBody]UpdateUserProfileContract contract)
        {
            var currentUserId = userIdProvider.GetUserId(User);
            var model = mapper.Map<UpdateUserProfileModel>(contract);
            model.UserId = currentUserId;
            var response = await aggregatorService.UpdateUserProfile(model);
            return mapper.Map<UserProfileContract>(response);
        }

        [HttpDelete("DeleteUserProfile")]
        public async Task<string> DeleteUserProfile()
        {
            var currentUserId = userIdProvider.GetUserId(User);
            var response = await aggregatorService.DeleteUserProfile(currentUserId);
            return response;
        }

        private async Task<UserProfileContract> GetUserProfileById(GetUserProfileModel model)
        {
            var response = await aggregatorService.GetUserProfile(model);
            return mapper.Map<UserProfileContract>(response);
        }

        #endregion

        #region Food

        [HttpGet("GetFood")]
        public async Task<UserFoodsContract> GetFood([FromQuery] GetUserFoodsContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<GetUserFoodsModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.GetFoods(model);
            return mapper.Map<UserFoodsContract>(response);
        }

        [HttpPut("AddFood")]
        public async Task<FoodItemContract> AddFood([FromBody] AddUserFoodContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<AddUserFoodModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.AddFood(model);
            return mapper.Map<FoodItemContract>(response);
        }

        [HttpPut("EditFood")]
        public async Task<FoodItemContract> EditFood([FromBody] UpdateUserFoodContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<UpdateUserFoodModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.EditFood(model);
            return mapper.Map<FoodItemContract>(response);
        }

        [HttpDelete("RemoveFood/{foodId}")]
        public async Task<string> RemoveFood([FromRoute] string foodId)
        {
            var userId = userIdProvider.GetUserId(User);
            var response = await aggregatorService.RemoveFood(userId, foodId);
            return mapper.Map<string>(response);
        }

        #endregion

        #region Exercises

        [HttpGet("GetExercises")]
        public async Task<UserExercisesContract> GetExercises([FromQuery] GetUserExercisesContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<GetUserExercisesModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.GetExercises(model);
            return mapper.Map<UserExercisesContract>(response);
        }

        [HttpPut("AddExercise")]
        public async Task<ExerciseItemContract> AddExercise([FromBody] AddUserExerciseContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<AddUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.AddExercise(model);
            return mapper.Map<ExerciseItemContract>(response);
        }

        [HttpPut("EditExercise")]
        public async Task<ExerciseItemContract> EditExercise([FromBody] UpdateUserExerciseContract contract)
        {
            var userId = userIdProvider.GetUserId(User);
            var model = mapper.Map<UpdateUserExerciseModel>(contract);
            model.UserId = userId;
            var response = await aggregatorService.EditExercise(model);
            return mapper.Map<ExerciseItemContract>(response);
        }

        [HttpDelete("RemoveExercise/{exerciseId}")]
        public async Task<string> RemoveExercise([FromRoute] string exerciseId)
        {
            var userId = userIdProvider.GetUserId(User);
            var response = await aggregatorService.RemoveExercise(userId, exerciseId);
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
                UserId = userIdProvider.GetUserId(User),
            };
            return aggregatorService.GetNotificationTicket(model);
        }

        [HttpGet("ValidateNotificationTicket")]
        public Task<bool> ValidateNotificationTicket([FromQuery] string ticket)
        {
            var model = new ValidateNotificationTicketModel
            {
                Ticket = ticket,
                Ip = GetRequestIp(),
                UserId = userIdProvider.GetUserId(User)
            };
            return aggregatorService.ValidateNotificationTicket(model);
        }

        private string GetRequestIp()
        {
            return Request.HttpContext.Connection.LocalIpAddress?.ToString();
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