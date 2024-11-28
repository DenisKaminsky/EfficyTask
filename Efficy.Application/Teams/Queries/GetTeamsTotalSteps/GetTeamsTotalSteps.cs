using Efficy.Application.Common.Interfaces;
using Efficy.Application.Teams.Queries.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Teams.Queries.GetTeamsTotalSteps;

public record GetTeamsTotalStepsQuery : IRequest<IEnumerable<TeamWithTotalStepsDto>>;

public class GetTeamsTotalStepsQueryHandler : IRequestHandler<GetTeamsTotalStepsQuery, IEnumerable<TeamWithTotalStepsDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetTeamsTotalStepsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TeamWithTotalStepsDto>> Handle(GetTeamsTotalStepsQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Teams
            .Select(x => new TeamWithTotalStepsDto(
                x.Id,
                x.Name,
                x.Counters.Sum(c => c.Value)))
            .ToArrayAsync(cancellationToken);

        return data;
    }
}