namespace Efficy.Application.Teams.Queries.GetAllTeams;

/// <summary>
/// Represents basic information about the Team
/// </summary>
/// <param name="Id">Id of the Team</param>
/// <param name="Name">Name of the Team</param>
public record TeamDto(int Id, string Name);