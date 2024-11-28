using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Exceptions;
using MediatR;

namespace Efficy.Application.Teams.Commands.DeleteTeam;

public record DeleteTeamCommand(int Id) : IRequest;

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeleteTeamCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Teams.FindAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException(request.Id.ToString());

        _dbContext.Teams.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}