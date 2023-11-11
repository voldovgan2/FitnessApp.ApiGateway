using System;

namespace FitnessApp.ApiGateway.Exceptions
{
    public class ForbidenException : Exception
    {
        public ForbidenException(string error) : base(error) { }
    }
}
