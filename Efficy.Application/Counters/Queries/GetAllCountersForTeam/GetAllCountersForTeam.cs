using Efficy.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Counters.Queries.GetAllCountersForTeam;

public record GetAllCountersForTeamQuery(int TeamId) : IRequest<IEnumerable<CounterForTeamDto>>;

public class GetAllCountersForTeamQueryHandler : IRequestHandler<GetAllCountersForTeamQuery, IEnumerable<CounterForTeamDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCountersForTeamQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CounterForTeamDto>> Handle(GetAllCountersForTeamQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Counters
            .Where(x => x.TeamId == request.TeamId)
            .Select(x => new CounterForTeamDto(x.Id, x.Name, x.Value))
            .ToArrayAsync(cancellationToken);

        return result;
    }
}