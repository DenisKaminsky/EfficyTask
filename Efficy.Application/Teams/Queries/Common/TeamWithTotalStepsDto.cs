namespace Efficy.Application.Teams.Queries.Common;

/// <summary>
/// Represents information about the Team including total steps count
/// </summary>
/// <param name="Id">Id of the Team</param>
/// <param name="Name">Name of the Team</param>
/// <param name="TotalSteps">Number of steps that Team have walked in total</param>
public record TeamWithTotalStepsDto(int Id, string Name, int TotalSteps);