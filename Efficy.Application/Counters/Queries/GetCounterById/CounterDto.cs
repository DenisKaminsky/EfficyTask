namespace Efficy.Application.Counters.Queries.GetCounterById;

/// <summary>
/// Represents full information about the Counter
/// </summary>
/// <param name="Id">Id of the Counter</param>
/// <param name="Name">Name of the Counter</param>
/// <param name="Value">Current value of the Counter</param>
/// <param name="TeamId">Id of the Team to which the Counter is assigned</param>
/// <param name="TeamName">Name of the Team to which the Counter is assigned</param>
public record CounterDto (int Id, string Name, int Value, int TeamId, string TeamName);