﻿using System.Diagnostics.CodeAnalysis;
using FitnessApp.Common.Paged.Models.Output;

namespace FitnessApp.ApiGateway.Models.Food.Output;

[ExcludeFromCodeCoverage]
public class UserFoodsModel
{
    public string UserId { get; set; }
    public PagedDataModel<FoodItemModel> Foods { get; set; }
}