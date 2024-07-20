using System;
using FitnessApp.ApiGateway.Models.File.Output;

namespace FitnessApp.ApiGateway.Models.Exercises.Output;

public class ExerciseItemModel : IFileModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
    public DateTime AddedDate { get; set; }
}
