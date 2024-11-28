using Efficy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Team> Teams { get; }

    DbSet<Counter> Counters { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
