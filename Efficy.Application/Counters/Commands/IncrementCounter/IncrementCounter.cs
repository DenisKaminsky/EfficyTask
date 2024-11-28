using MediatR;
using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Exceptions;

namespace Efficy.Application.Counters.Commands.IncrementCounter
{
    public record IncrementCounterCommand(int Id, int IncrementValue) : IRequest<int>;

    public class IncrementCounterCommandHandler : IRequestHandler<IncrementCounterCommand, int>
    {
        private readonly IAppDbContext _dbContext;

        public IncrementCounterCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(IncrementCounterCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Counters.FindAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException(request.Id.ToString());

            entity.Value += request.IncrementValue;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Value;
        }
    }
}
