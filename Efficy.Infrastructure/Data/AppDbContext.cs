using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Team> Teams => Set<Team>();
}