using Efficy.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Counters.Commands.CreateCounter;

public class CreateCounterCommandValidator : AbstractValidator<CreateCounterCommand>
{
    private readonly IAppDbContext _dbContext;

    public CreateCounterCommandValidator(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
        
        RuleFor(x => x.TeamId)
            .MustAsync(HasValidTeamId)
                .WithMessage("Team with the specified Id does not exist.")
                .WithErrorCode("Invalid");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(HasUniqueNameForTeam)
                .WithMessage("Team already has a counter with this name.")
                .WithErrorCode("Duplicate");
    }

    public async Task<bool> HasValidTeamId(int teamId, CancellationToken cancellationToken)
    {
        return await _dbContext.Teams
            .AnyAsync(x => x.Id == teamId, cancellationToken);
    }

    public async Task<bool> HasUniqueNameForTeam(CreateCounterCommand command, string name, CancellationToken cancellationToken)
    {
        return !await _dbContext.Counters
            .AnyAsync(x => 
                    x.Name == name 
                    && x.TeamId == command.TeamId, 
                cancellationToken);
    }
}