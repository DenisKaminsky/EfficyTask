using Efficy.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Teams.Queries.GetAllTeams;

public record GetAllTeamsQuery : IRequest<IEnumerable<TeamDto>>;

public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllTeamsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Teams
            .Select(x => new TeamDto(x.Id, x.Name))
            .ToArrayAsync(cancellationToken);

        return result;
    }
}
