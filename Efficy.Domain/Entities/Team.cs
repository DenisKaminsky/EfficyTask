namespace Efficy.Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; }

    public IList<Counter> Counters { get; private set; } = new List<Counter>();
}