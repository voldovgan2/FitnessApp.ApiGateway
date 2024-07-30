using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Contracts.Food.Input;

[ExcludeFromCodeCoverage]
public class AddUserFoodContract
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
}
