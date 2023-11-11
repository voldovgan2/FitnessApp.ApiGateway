using System;

namespace FitnessApp.ApiGateway.Exceptions
{
    public class InternalUnAuthorizedException : Exception
    {
        public InternalUnAuthorizedException(string error) : base(error) { }
    }
}
