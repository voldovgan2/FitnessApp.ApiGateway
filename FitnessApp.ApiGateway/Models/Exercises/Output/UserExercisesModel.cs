using FitnessApp.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Models.Exercises.Output
{
    public class UserExercisesModel
    {
        public string UserId { get; set; }
        public PagedDataModel<ExerciseItemModel> Exercises { get; set; }
    }
}