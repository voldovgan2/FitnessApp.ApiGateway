using System.Diagnostics.CodeAnalysis;
using FitnessApp.Common.Paged.Models.Input;

namespace FitnessApp.ApiGateway.Models.Food.Input;

[ExcludeFromCodeCoverage]
public class GetUserFoodsModel : GetPagedDataModel
{
    public string UserId { get; set; }
}