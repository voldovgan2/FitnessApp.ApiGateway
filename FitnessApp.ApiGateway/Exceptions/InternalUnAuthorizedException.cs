using System;
using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Exceptions;

[ExcludeFromCodeCoverage]
public class InternalUnAuthorizedException(string error) : Exception(error);
