using Efficy.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Efficy.Infrastructure.Data.Configuration;

public class CounterConfiguration : IEntityTypeConfiguration<Counter>
{
    public void Configure(EntityTypeBuilder<Counter> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne<Team>(x => x.Team)
            .WithMany(x => x.Counters)
            .HasForeignKey(x => x.TeamId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
    }
}