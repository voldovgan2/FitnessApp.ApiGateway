namespace FitnessApp.ApiGateway.Contracts.Food.Input
{
    public class UpdateUserFoodContract
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public double Calories { get; set; }
    }
}