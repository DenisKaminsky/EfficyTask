using Efficy.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Teams.Commands.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    private readonly IAppDbContext _dbContext;

    public CreateTeamCommandValidator(IAppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(HasUniqueName)
                .WithMessage("Team with this '{PropertyName}' already exists.")
                .WithErrorCode("Duplicate");
    }

    public async Task<bool> HasUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _dbContext.Teams
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}