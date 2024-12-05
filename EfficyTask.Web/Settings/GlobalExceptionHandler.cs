using Efficy.Application.Common.Exceptions;
using Efficy.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EfficyTask.Web.Settings
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _customExceptionHandlers;

        public GlobalExceptionHandler()
        {
            // Register known exception types and handlers.
            _customExceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException }
            };
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionType = exception.GetType();

            if (_customExceptionHandlers.ContainsKey(exceptionType))
            {
                await _customExceptionHandlers[exceptionType].Invoke(httpContext, exception);
            }
            else if (exception is OperationCanceledException or TaskCanceledException)
            {
                await HandleOperationCancelledException(httpContext);
            }
            else
            {
                await HandleGlobalInternalException(httpContext, exception);
            }

            return true;
        }

        private async Task HandleValidationException(HttpContext httpContext, Exception ex)
        {
            var exception = (ValidationException)ex;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            });
        }

        private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
        {
            var exception = (NotFoundException)ex;

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            });
        }

        private async Task HandleOperationCancelledException(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Operation was cancelled."
            });
        }

        private async Task HandleGlobalInternalException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "An unhandled exception has occurred while executing the request.",
                Detail = ex.Message
            });
        }
    }
}
