using Efficy.Application.Common.Interfaces;
using Efficy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Efficy.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Team> Teams => Set<Team>();

    public DbSet<Counter> Counters => Set<Counter>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}