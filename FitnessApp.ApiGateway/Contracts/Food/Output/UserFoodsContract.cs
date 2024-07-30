using System.Diagnostics.CodeAnalysis;
using FitnessApp.Common.Paged.Contracts.Output;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Contracts.Food.Output;

[ExcludeFromCodeCoverage]
public class UserFoodsContract
{
    [JsonRequired]
    public string UserId { get; set; }
    [JsonRequired]
    public PagedDataContract<FoodItemContract> Foods { get; set; }
}