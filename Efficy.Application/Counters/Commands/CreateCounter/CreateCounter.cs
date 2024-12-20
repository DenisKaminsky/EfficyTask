﻿using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Entities;
using MediatR;

namespace Efficy.Application.Counters.Commands.CreateCounter;

/// <summary>
/// Represents a request to create a Counter
/// </summary>
/// <param name="TeamId">Id of the Team to which the Counter will be added</param>
/// <param name="Name">Name of the Counter. Must be unique within the Team</param>
public record CreateCounterCommand(int TeamId, string Name) : IRequest<int>;

public class CreateCounterCommandHandler : IRequestHandler<CreateCounterCommand, int>
{
    private readonly IAppDbContext _dbContext;

    public CreateCounterCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateCounterCommand request, CancellationToken cancellationToken)
    {
        var entity = new Counter
        {
            Name = request.Name!,
            TeamId = request.TeamId,
            Value = 0
        };

        await _dbContext.Counters.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}