using MediatR;
using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Counters.Commands.IncrementCounter
{
    /// <summary>
    /// Represents a request to increment the Counter
    /// </summary>
    /// <param name="CounterId">Id of the Counter we want to update</param>
    /// <param name="IncrementValue">Value by which we want to increment the counter. Must be in the range from 1 to 100 inclusive</param>
    public record IncrementCounterCommand(int CounterId, int IncrementValue) : IRequest<int>;

    public class IncrementCounterCommandHandler : IRequestHandler<IncrementCounterCommand, int>
    {
        private readonly IAppDbContext _dbContext;

        public IncrementCounterCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(IncrementCounterCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Counters
                .Where(x => x.Id == request.CounterId)
                .ExecuteUpdateAsync(s => 
                    s.SetProperty(c => c.Value, c => c.Value + request.IncrementValue), 
                    cancellationToken);

            var counterValue = await _dbContext.Counters
                .Where(x => x.Id == request.CounterId)
                .Select(x => new { Value = x.Value })
                .SingleOrDefaultAsync(cancellationToken);
            if (counterValue == null)
                throw new NotFoundException(request.CounterId.ToString());

            return counterValue.Value;
        }
    }
}
