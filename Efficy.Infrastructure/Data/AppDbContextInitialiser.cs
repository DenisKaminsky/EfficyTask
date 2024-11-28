using Efficy.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Infrastructure.Data;

public class AppDbContextInitializer
{
    private readonly ILogger<AppDbContextInitializer> _logger;
    private readonly AppDbContext _context;

    public AppDbContextInitializer(ILogger<AppDbContextInitializer> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Teams.Any())
        {
            _context.Teams.Add(new Team
            {
                Name = "Software Engineers",
                Counters =
                {
                    new Counter { Name = "Denis Counter", Value = 110 },
                    new Counter { Name = "Olivia Counter", Value = 75 },
                    new Counter { Name = "Paul Counter", Value = 59 }
                }
            });

            _context.Teams.Add(new Team
            {
                Name = "Quality Assurance",
                Counters =
                {
                    new Counter { Name = "Olivia Counter", Value = 40 },
                    new Counter { Name = "Mike Counter", Value = 5 },
                    new Counter { Name = "Amelia Counter", Value = 34 },
                    new Counter { Name = "Jake Counter", Value = 11 },
                    new Counter { Name = "Sophie Counter", Value = 20 }
                }
            });

            _context.Teams.Add(new Team
            {
                Name = "Product Owners",
                Counters =
                {
                    new Counter { Name = "Scott Counter", Value = 23 },
                    new Counter { Name = "Jacob Counter", Value = 15 },
                    new Counter { Name = "William Counter", Value = 60 }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
