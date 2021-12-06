using FitnessApp.Paged.Contracts.Output;

namespace FitnessApp.ApiGateway.Contracts.Exercises.Output
{
    public class UserExercisesContract
    {
        public string UserId { get; set; }
        public PagedDataContract<ExerciseItemContract> Exercises { get; set; }
    }
}