using FitnessApp.ApiGateway.Services.Abstractions.Collection;

namespace FitnessApp.ApiGateway.Services.Food
{
    public interface IFoodService<Model, CollectionItem> : ICollectionService<Model, CollectionItem>
    {
    }
}