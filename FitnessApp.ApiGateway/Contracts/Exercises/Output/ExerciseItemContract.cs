using System;
using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Contracts.Exercises.Output;

[ExcludeFromCodeCoverage]
public class ExerciseItemContract
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
    public DateTime AddedDate { get; set; }
}
