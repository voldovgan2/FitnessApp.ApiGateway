﻿using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Models.Food.Input;

[ExcludeFromCodeCoverage]
public class UpdateUserFoodModel
{
    public string UserId { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
}
