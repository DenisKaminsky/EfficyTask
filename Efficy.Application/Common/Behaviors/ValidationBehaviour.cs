using FluentValidation;
using MediatR;
using ValidationException = Efficy.Application.Common.Exceptions.ValidationException;

namespace Efficy.Application.Common.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(
                _validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var errors = results
                .Where(x => x.Errors.Any())
                .SelectMany(x => x.Errors)
                .ToArray();

            if (errors.Any())
                throw new ValidationException(errors);
        }

        return await next();
    }
}