﻿using System;
using Newtonsoft.Json;

namespace FitnessApp.ApiGateway.Contracts.Food.Output
{
    public class FoodItemContract
    {
        [JsonRequired]
        public string Id { get; set; }
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string Description { get; set; }
        [JsonRequired]
        public string Photo { get; set; }
        [JsonRequired]
        public double Calories { get; set; }
        [JsonRequired]
        public DateTime AddedDate { get; set; }
    }
}
