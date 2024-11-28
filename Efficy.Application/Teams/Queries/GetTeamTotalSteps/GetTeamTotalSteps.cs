using Efficy.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Efficy.Application.Teams.Queries.Common;
using Efficy.Domain.Exceptions;

namespace Efficy.Application.Teams.Queries.GetTeamTotalSteps;

public record GetTeamTotalStepsQuery(int TeamId) : IRequest<TeamWithTotalStepsDto>;

public class GetTeamTotalStepsQueryHandler : IRequestHandler<GetTeamTotalStepsQuery, TeamWithTotalStepsDto>
{
    private readonly IAppDbContext _dbContext;

    public GetTeamTotalStepsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TeamWithTotalStepsDto> Handle(GetTeamTotalStepsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Teams
            .Where(x => x.Id == request.TeamId)
            .Select(x => new TeamWithTotalStepsDto(
                x.Id, 
                x.Name, 
                x.Counters.Sum(c => c.Value)))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException(request.TeamId.ToString());

        return entity;
    }
}