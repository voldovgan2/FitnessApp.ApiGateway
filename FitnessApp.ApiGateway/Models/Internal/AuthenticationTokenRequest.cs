namespace FitnessApp.ApiGateway.Models.Internal
{
    public class AuthenticationTokenRequest
    {
        public string ApiName { get; set; }
        public string Scope { get; set; }
    }
}
