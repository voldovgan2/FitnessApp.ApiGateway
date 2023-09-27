using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Services.Abstractions.Collection;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Food
{
    public class FoodService
        : CollectionService<UserFoodsModel, FoodItemModel>,
        IFoodService
    {
        private const string API = "Food";
        private const string GET_FOOD_METHOD = "GetFood";
        private const string ADD_FOOD_METHOD = "AddFood";
        private const string EDIT_FOOD_METHOD = "EditFood";
        private const string REMOVE_FOOD_METHOD = "RemoveFood";

        private readonly ApiClientSettings _apiClientSettings;

        public FoodService(
            ApiClientSettings apiClientSettings,
            IInternalClient internalClient)
            : base(apiClientSettings, internalClient)
        {
            _apiClientSettings = apiClientSettings;
        }

        public Task<UserFoodsModel> GetFoods(GetUserFoodsModel model)
        {
            return GetItem(_apiClientSettings.Url, API, GET_FOOD_METHOD, model);
        }

        public Task<FoodItemModel> AddFood(AddUserFoodModel model)
        {
            return AddItem(_apiClientSettings.Url, API, ADD_FOOD_METHOD, model);
        }

        public Task<FoodItemModel> EditFood(UpdateUserFoodModel model)
        {
            return EditItem(_apiClientSettings.Url, API, EDIT_FOOD_METHOD, model);
        }

        public Task<string> RemoveFood(string userId, string foodId)
        {
            return RemoveItem(_apiClientSettings.Url, API, REMOVE_FOOD_METHOD, userId, foodId);
        }
    }
}