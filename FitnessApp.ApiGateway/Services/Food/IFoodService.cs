using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Food.Input;
using FitnessApp.ApiGateway.Models.Food.Output;

namespace FitnessApp.ApiGateway.Services.Food;

public interface IFoodService
{
    Task<UserFoodsModel> GetFoods(GetUserFoodsModel model);
    Task<FoodItemModel> AddFood(AddUserFoodModel model);
    Task<FoodItemModel> EditFood(UpdateUserFoodModel model);
    Task<string> RemoveFood(string userId, string foodId);
}