using FitnessApp.Common.Paged.Contracts.Output;

namespace FitnessApp.ApiGateway.Contracts.Food.Output
{
    public class UserFoodsContract
    {
        public string UserId { get; set; }
        public PagedDataContract<FoodItemContract> Foods { get; set; }
    }
}