﻿using System;
using System.Diagnostics.CodeAnalysis;
using FitnessApp.ApiGateway.Models.File.Output;

namespace FitnessApp.ApiGateway.Models.Food.Output;

[ExcludeFromCodeCoverage]
public class FoodItemModel : IFileModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
    public DateTime AddedDate { get; set; }
}