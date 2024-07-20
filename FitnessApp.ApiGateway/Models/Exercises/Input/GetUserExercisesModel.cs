using FitnessApp.Common.Paged.Models.Input;

namespace FitnessApp.ApiGateway.Models.Exercises.Input;

public class GetUserExercisesModel : GetPagedDataModel
{
    public string UserId { get; set; }
}