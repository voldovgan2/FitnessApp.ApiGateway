using FitnessApp.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Models.Food.Output
{
    public class UserFoodsModel
    {
        public string UserId { get; set; }
        public PagedDataModel<FoodItemModel> Foods { get; set; }
    }
}