using MediatR;
using Microsoft.Extensions.Logging;

namespace Efficy.Application.Common.Behaviors;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Unhandled exception for Request {RequestName} {RequestContent}", requestName, request);

            throw;
        }
    }
}