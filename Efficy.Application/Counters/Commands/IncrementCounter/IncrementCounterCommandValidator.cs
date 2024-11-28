using FluentValidation;

namespace Efficy.Application.Counters.Commands.IncrementCounter;

public class IncrementCounterCommandValidator: AbstractValidator<IncrementCounterCommand>
{
    public IncrementCounterCommandValidator()
    {
        RuleFor(x => x.IncrementValue)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}