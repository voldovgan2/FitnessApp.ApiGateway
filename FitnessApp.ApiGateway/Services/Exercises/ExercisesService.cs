using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Models.Exercises.Input;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Services.Abstractions.Collection;
using FitnessApp.ApiGateway.Services.InternalClient;

namespace FitnessApp.ApiGateway.Services.Exercises
{
    public class ExercisesService
        : CollectionService<UserExercisesModel, ExerciseItemModel>,
        IExercisesService
    {
        private const string API = "Exercises";
        private const string GET_EXERCISES_METHOD = "GetExercises";
        private const string ADD_EXERCISE_METHOD = "AddExercise";
        private const string EDIT_EXERCISE_METHOD = "EditExercise";
        private const string REMOVE_EXERCISE_METHOD = "RemoveExercise";

        private readonly ApiClientSettings _apiClientSettings;

        public ExercisesService(
            ApiClientSettings apiClientSettings,
            IInternalClient internalClient)
            : base(apiClientSettings, internalClient)
        {
            _apiClientSettings = apiClientSettings;
        }

        public Task<UserExercisesModel> GetExercises(GetUserExercisesModel model)
        {
            return GetItem(_apiClientSettings.Url, API, GET_EXERCISES_METHOD, model);
        }

        public Task<ExerciseItemModel> AddExercise(AddUserExerciseModel model)
        {
            return AddItem(_apiClientSettings.Url, API, ADD_EXERCISE_METHOD, model);
        }

        public Task<ExerciseItemModel> EditExercise(UpdateUserExerciseModel model)
        {
            return EditItem(_apiClientSettings.Url, API, EDIT_EXERCISE_METHOD, model);
        }

        public Task<string> RemoveExercise(string userId, string exerciseId)
        {
            return RemoveItem(_apiClientSettings.Url, API, REMOVE_EXERCISE_METHOD, userId, exerciseId);
        }
    }
}