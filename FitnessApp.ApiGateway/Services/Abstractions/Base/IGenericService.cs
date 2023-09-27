using System.Threading.Tasks;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Services.Abstractions.Base
{
    public interface IGenericService<TModel>
    {
        Task<PagedDataModel<TModel>> GetItems(string baseUrl, string api, string methodName, object payload);
        Task<TModel> GetItem(string baseUrl, string api, string methodName, string userId);
        Task<TModel> CreateItem(string baseUrl, string api, string methodName, object payload);
        Task<TModel> UpdateItem(string baseUrl, string api, string methodName, object payload);
        Task<string> DeleteItem(string baseUrl, string api, string methodName, string userId);
    }
}