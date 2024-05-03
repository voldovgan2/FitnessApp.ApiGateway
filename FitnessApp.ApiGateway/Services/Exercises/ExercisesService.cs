using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Services.Abstractions;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Exercises
{
    public class ExercisesService(
        ApiClientSettings apiClientSettings,
        IInternalClient internalClient)
        : CollectionService<UserExercisesModel, ExerciseItemModel>(
            apiClientSettings,
            internalClient),
        IExercisesService
    {
        private const string API = "Exercises";
        private const string GET_EXERCISES_METHOD = "GetExercises";
        private const string ADD_EXERCISE_METHOD = "AddExercise";
        private const string EDIT_EXERCISE_METHOD = "EditExercise";
        private const string REMOVE_EXERCISE_METHOD = "RemoveExercise";

        public Task<UserExercisesModel> GetExercises(GetUserExercisesModel model)
        {
            return GetItem(ApiClientSettings.Url, API, GET_EXERCISES_METHOD, model);
        }

        public Task<ExerciseItemModel> AddExercise(AddUserExerciseModel model)
        {
            return AddItem(ApiClientSettings.Url, API, ADD_EXERCISE_METHOD, model);
        }

        public Task<ExerciseItemModel> EditExercise(UpdateUserExerciseModel model)
        {
            return EditItem(ApiClientSettings.Url, API, EDIT_EXERCISE_METHOD, model);
        }

        public Task<string> RemoveExercise(string userId, string exerciseId)
        {
            return RemoveItem(ApiClientSettings.Url, API, REMOVE_EXERCISE_METHOD, userId, exerciseId);
        }
    }
}