﻿using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Exceptions;
using MediatR;

namespace Efficy.Application.Counters.Commands.DeleteCounter;

public record DeleteCounterCommand(int Id) : IRequest;

public class DeleteCounterCommandHandler : IRequestHandler<DeleteCounterCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeleteCounterCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteCounterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Counters.FindAsync(request.Id, cancellationToken);
            
        if (entity == null)
            throw new NotFoundException(request.Id.ToString());

        _dbContext.Counters.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}