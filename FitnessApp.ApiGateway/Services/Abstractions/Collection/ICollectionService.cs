using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Abstractions.Collection
{
    public interface ICollectionService<Model, CollectionItem>
    {
        Task<Model> GetItemAsync(object model);
        Task<CollectionItem> AddItemAsync(object model);
        Task<CollectionItem> EditItemAsync(object model);
        Task<string> RemoveItemAsync(string userId, string id);
    }
}
