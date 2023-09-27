using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Abstractions.Collection
{
    public interface ICollectionService<TModel, TCollectionItem>
    {
        Task<TModel> GetItem(string baseUrl, string api, string methodName, object payload);
        Task<TCollectionItem> AddItem(string baseUrl, string api, string methodName, object payload);
        Task<TCollectionItem> EditItem(string baseUrl, string api, string methodName, object payload);
        Task<string> RemoveItem(string baseUrl, string api, string methodName, string userId, string id);
    }
}
