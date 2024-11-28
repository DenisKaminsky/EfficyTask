using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Entities;
using MediatR;

namespace Efficy.Application.Teams.Commands.CreateTeam;

/// <summary>
/// Represents a request to create a Team
/// </summary>
/// <param name="Title">Name of the Team. Should be Unique</param>
public record CreateTeamCommand(string? Title) : IRequest<int>;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, int>
{
    private readonly IAppDbContext _dbContext;

    public CreateTeamCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var teamEntity = new Team
        {
            Name = request.Title!
        };

        await _dbContext.Teams.AddAsync(teamEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return teamEntity.Id;
    }
}
