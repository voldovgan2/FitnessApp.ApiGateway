using System;
using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Exceptions;

[ExcludeFromCodeCoverage]
public class ForbidenException(string error) : Exception(error);
