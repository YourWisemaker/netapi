using System.Net;
using System.Text.Json;
using netapi.Exceptions;

namespace netapi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            if (exception is ApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = apiException.Message;
            }
            else if (exception is HttpRequestException httpEx)
            {
                statusCode = httpEx.StatusCode ?? HttpStatusCode.InternalServerError;
                message = $"External API error: {httpEx.Message}";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = (int)statusCode,
                message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
