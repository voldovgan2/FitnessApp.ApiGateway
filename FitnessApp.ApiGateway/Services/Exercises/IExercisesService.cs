using System.Threading.Tasks;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;

namespace FitnessApp.ApiGateway.Services.Exercises
{
    public interface IExercisesService
    {
        Task<UserExercisesModel> GetExercises(GetUserExercisesModel model);
        Task<ExerciseItemModel> AddExercise(AddUserExerciseModel model);
        Task<ExerciseItemModel> EditExercise(UpdateUserExerciseModel model);
        Task<string> RemoveExercise(string userId, string exerciseId);
    }
}