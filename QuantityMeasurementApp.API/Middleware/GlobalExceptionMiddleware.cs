using System.Net;
using System.Text.Json;
using SystemException = System.Exception;

namespace QuantityMeasurementApp.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, SystemException ex)
        {
            var (status, error) = ex switch
            {
                System.UnauthorizedAccessException              => (HttpStatusCode.Unauthorized,        "Unauthorized"),
                System.InvalidOperationException                => (HttpStatusCode.BadRequest,          "Bad Request"),
                System.ArgumentException                        => (HttpStatusCode.BadRequest,          "Bad Request"),
                System.NotSupportedException                    => (HttpStatusCode.BadRequest,          "Bad Request"),
                System.Collections.Generic.KeyNotFoundException => (HttpStatusCode.NotFound,            "Not Found"),
                _                                               => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };

            var response = new
            {
                timestamp = DateTime.UtcNow,
                status    = (int)status,
                error,
                message   = ex.Message,
                path      = context.Request.Path.Value
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)status;

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(response,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
        }
    }
}
