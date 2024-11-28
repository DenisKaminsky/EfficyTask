namespace Efficy.Application.Counters.Queries.GetAllCountersForTeam;

/// <summary>
/// Represents Counter information for a specific Team
/// </summary>
/// <param name="Id">Id of the Counter</param>
/// <param name="Title">Name of the Counter</param>
/// <param name="Value">Current value of the Counter</param>
public record CounterForTeamDto(int Id, string Title, int Value);