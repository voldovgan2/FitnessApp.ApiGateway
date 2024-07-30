using System.Diagnostics.CodeAnalysis;
using FitnessApp.Common.Paged.Models.Input;

namespace FitnessApp.ApiGateway.Models.Exercises.Input;

[ExcludeFromCodeCoverage]
public class GetUserExercisesModel : GetPagedDataModel
{
    public string UserId { get; set; }
}