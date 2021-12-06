namespace FitnessApp.ApiGateway.Models.Food.Input
{
    public class AddUserFoodModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public double Calories { get; set; }
    }
}