﻿namespace FitnessApp.ApiGateway.Contracts.Exercises.Input;

public class UpdateUserExerciseContract
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public double Calories { get; set; }
}