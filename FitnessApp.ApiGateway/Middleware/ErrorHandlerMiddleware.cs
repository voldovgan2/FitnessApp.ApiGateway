using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using FitnessApp.Common.Middleware;
using Microsoft.AspNetCore.Http;

namespace FitnessApp.ApiGateway.Middleware;

[ExcludeFromCodeCoverage]
public class ErrorHandlerMiddleware(RequestDelegate next) : AbstractErrorHandlerMiddleware(next)
{
    protected override Task HandleGlobalError(HttpContext context, Exception error)
    {
        return Task.CompletedTask;
    }

    protected override HttpStatusCode GetStatusCodeByError(Exception error)
    {
        return HttpStatusCode.InternalServerError;
    }
}
