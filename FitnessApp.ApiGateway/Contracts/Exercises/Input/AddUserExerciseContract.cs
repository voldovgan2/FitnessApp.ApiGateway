using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Contracts.Exercises.Input;

[ExcludeFromCodeCoverage]
public class AddUserExerciseContract
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
}