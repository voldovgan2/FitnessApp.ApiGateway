using System;
using System.Net;
using System.Threading.Tasks;
using FitnessApp.Common.Middleware;
using FitnessApp.Common.Serializer.JsonSerializer;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace FitnessApp.ApiGateway.Middleware
{
    public class ErrorHandlerMiddleware : AbstractErrorHandlerMiddleware
    {
        public ErrorHandlerMiddleware(
            RequestDelegate next,
            IJsonSerializer serializer)
            : base(next, serializer) { }

        protected override Task HandleGlobalError(HttpContext context, Exception error)
        {
            return Task.CompletedTask;
        }

        protected override HttpStatusCode GetStatusCodeByError(Exception error)
        {
            return HttpStatusCode.InternalServerError;
        }
    }
}
