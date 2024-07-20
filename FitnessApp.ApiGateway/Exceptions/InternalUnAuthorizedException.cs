using System;

namespace FitnessApp.ApiGateway.Exceptions;

public class InternalUnAuthorizedException(string error) : Exception(error);
