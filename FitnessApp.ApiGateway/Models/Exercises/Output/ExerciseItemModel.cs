using System;
using FitnessApp.ApiGateway.Models.Blob.Output;

namespace FitnessApp.ApiGateway.Models.Exercises.Output
{
    public class ExerciseItemModel : IBlobModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public double Calories { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
