﻿using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Exceptions;
using MediatR;

namespace Efficy.Application.Teams.Commands.DeleteTeam;

/// <summary>
/// Represents a request to delete the Team
/// </summary>
/// <param name="TeamId">Id of the Team to delete</param>
public record DeleteTeamCommand(int TeamId) : IRequest;

public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeleteTeamCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Teams.FindAsync(request.TeamId, cancellationToken);
        if (entity == null)
            throw new NotFoundException(request.TeamId.ToString());

        _dbContext.Teams.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}