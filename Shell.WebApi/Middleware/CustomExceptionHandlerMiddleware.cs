using System.Net;
using System.Text.Json;
using FluentValidation;
using Shell.Application.Common.Exceptions;

namespace Shell.WebApi.Middleware
{
    public class CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = validationException.Errors.First().ErrorMessage });
                    break;
                case DuplicateException duplicateException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = duplicateException.Message });
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = notFoundException.Message });
                    break;
                case CopyException createException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { error = createException.Message });
                    break;
                case CreateException
                    or UpdateException
                    or DeleteException:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { error = "Internal Server Error" });
                    break;
                default:
                    result = JsonSerializer.Serialize(new { error = exception.Message });
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
