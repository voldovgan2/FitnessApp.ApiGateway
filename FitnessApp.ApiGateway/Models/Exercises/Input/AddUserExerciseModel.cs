namespace FitnessApp.ApiGateway.Models.Exercises.Input
{
    public class AddUserExerciseModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public double Calories { get; set; }
    }
}