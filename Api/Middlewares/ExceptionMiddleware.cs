using Application.Ult;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            context.Response.ContentType = "application/json";

            object response;
            var innerException = ex.InnerException ?? ex;
            switch (innerException)
            {
                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new ApiResponse<string>
                    {
                        Message = argEx.Message,
                        Data = null,
                        Status = HttpStatusCode.BadRequest
                    };
                    break;

                case UnauthorizedAccessException authEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response = new ApiResponse<string>
                    {
                        Message = "Please log in to access this resource",
                        Data = null,
                        Status = HttpStatusCode.Unauthorized
                    };
                    break;
                case ForbiddenAccessException forbiddenEx: // Nếu bạn có custom exception
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response = new ApiResponse<string>
                    {
                        Message = "You do not have permission to access this resource",
                        Data = null,
                        Status = HttpStatusCode.Forbidden
                    };
                    break;
                case NotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new ApiResponse<string>
                    {
                        Message = Application.Ult.Constant.FAIL_READ_MSG,
                        Data = null,
                        Status = Application.Ult.Constant.FAIL_READ_CODE
                    };
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = _env.IsDevelopment()
                        ? new ApiResponse<string>
                        {
                            Message = ex.Message,
                            Data = ex.StackTrace,
                            Status = HttpStatusCode.InternalServerError
                        }
                         : new ApiResponse<string>
                         {
                             Message = "Something went wrong.",
                             Data = null,
                             Status = HttpStatusCode.InternalServerError
                         };

                    break;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string entityName, object key)
            : base($"{entityName} with key '{key}' was not found.") { }
    }
}
