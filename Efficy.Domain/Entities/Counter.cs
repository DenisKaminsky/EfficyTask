namespace Efficy.Domain.Entities;

public class Counter: BaseEntity
{
    public string Name { get; set; }

    public int Value { get; set; }

    public int TeamId { get; set; }

    public Team Team { get; set; } = null!;
}