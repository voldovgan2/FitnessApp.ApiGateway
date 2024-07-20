namespace FitnessApp.ApiGateway.Contracts.Exercises.Input;

public class AddUserExerciseContract
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
}