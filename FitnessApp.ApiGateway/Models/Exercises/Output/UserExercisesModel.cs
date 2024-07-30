using System.Diagnostics.CodeAnalysis;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Models.Exercises.Output;

[ExcludeFromCodeCoverage]
public class UserExercisesModel
{
    public string UserId { get; set; }
    public PagedDataModel<ExerciseItemModel> Exercises { get; set; }
}