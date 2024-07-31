using System;
using System.Diagnostics.CodeAnalysis;

namespace FitnessApp.ApiGateway.Exceptions;

[ExcludeFromCodeCoverage]
public class InternalUnAuthorizedException(Exception ex) : Exception(null, ex);