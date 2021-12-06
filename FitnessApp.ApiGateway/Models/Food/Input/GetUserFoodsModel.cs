using FitnessApp.Paged.Models.Input;

namespace FitnessApp.ApiGateway.Models.Food.Input
{
    public class GetUserFoodsModel : GetPagedDataModel
    {
        public string UserId { get; set; }
    }
}