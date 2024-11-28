using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Counters.Queries.GetCounterById;

public record GetCounterByIdQuery(int Id) : IRequest<CounterDto>;

public class GetCounterByIdQueryHandler : IRequestHandler<GetCounterByIdQuery, CounterDto>
{
    private readonly IAppDbContext _dbContext;

    public GetCounterByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CounterDto> Handle(GetCounterByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Counters
            .Where(x => x.Id == request.Id)
            .Select(x => new CounterDto(x.Id, x.Name, x.Value, x.TeamId, x.Team.Name))
            .SingleOrDefaultAsync(cancellationToken);

        if (result == null)
            throw new NotFoundException(request.Id.ToString());

        return result;
    }
}