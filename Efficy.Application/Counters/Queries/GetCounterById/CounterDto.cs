namespace Efficy.Application.Counters.Queries.GetCounterById;

public record CounterDto (int Id, string Name, int Value, int TeamId, string TeamName);