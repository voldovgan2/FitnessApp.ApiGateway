using FitnessApp.ApiGateway.Services.Abstractions.Collection;

namespace FitnessApp.ApiGateway.Services.Exercises
{
    public interface IExercisesService<Model, CollectionItem> : ICollectionService<Model, CollectionItem>
    {
    }
}
