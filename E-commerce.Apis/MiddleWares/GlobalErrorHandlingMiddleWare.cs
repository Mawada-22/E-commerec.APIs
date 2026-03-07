using System;
using System.Net;
using Domain.Exceptions;
using Shared.Error_Models;

namespace E_commerce.Apis.MiddleWares
{
    public class GlobalErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleWare> _logger;

        public GlobalErrorHandlingMiddleWare(
            RequestDelegate next,
            ILogger<GlobalErrorHandlingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                //not found endpoint handling
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await HandelNotFountEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandelNotFountEndPointAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var errorDetails = new ErrorDetails
                (
               message: $"The End Point {httpContext.Request.Path} Is Not Found",
               code:(int)HttpStatusCode.NotFound
                 ).ToString();

            await httpContext.Response.WriteAsync(errorDetails);
        }

        private static async Task HandleExceptionAsync(HttpContext context,Exception exception)
        {
            context.Response.ContentType = "application/json";

            // 500 is the correct status for unhandled server errors
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorDetails = new ErrorDetails(
                message: exception.Message,
                code: context.Response.StatusCode
            );

            await context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
