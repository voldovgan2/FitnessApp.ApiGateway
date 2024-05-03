using System;

namespace FitnessApp.ApiGateway.Exceptions
{
    public class ForbidenException(string error) : Exception(error);
}
