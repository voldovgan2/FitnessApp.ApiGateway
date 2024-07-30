using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace FitnessApp.ApiGateway.Models.Internal;

[ExcludeFromCodeCoverage]
public class InternalRequest
{
    public HttpMethod HttpMethod { get; }
    public string BaseUrl { get; }
    public string Api { get; }
    public string Method { get; }
    public string[] Routes { get; }
    public object Payload { get; }
    public object Query { get; }

    public InternalRequest(HttpMethod httpMethod, string baseUrl, string api, string method, string[] routes, object payload, object query)
    {
        HttpMethod = httpMethod;
        BaseUrl = baseUrl;
        Api = api;
        Method = method;
        Routes = routes;
        Payload = payload;
        Query = query;
    }
}