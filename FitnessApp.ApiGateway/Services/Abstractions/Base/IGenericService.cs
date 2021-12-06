using FitnessApp.Paged.Models.Output;
using System.Threading.Tasks;

namespace FitnessApp.ApiGateway.Services.Abstractions.Base
{
    public interface IGenericService<Model>
    {
        Task<PagedDataModel<Model>> GetItemsAsync(object model);
        Task<Model> GetItemAsync(string userId);
        Task<Model> CreateItemAsync(object model);
        Task<Model> UpdateItemAsync(object model);
        Task<string> DeleteItemAsync(string userId);
    }
}