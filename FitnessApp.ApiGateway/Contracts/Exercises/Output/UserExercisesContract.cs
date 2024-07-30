using System.Diagnostics.CodeAnalysis;
using FitnessApp.Common.Paged.Contracts.Output;

namespace FitnessApp.ApiGateway.Contracts.Exercises.Output;

[ExcludeFromCodeCoverage]
public class UserExercisesContract
{
    public string UserId { get; set; }
    public PagedDataContract<ExerciseItemContract> Exercises { get; set; }
}