using Efficy.Application.Teams.Commands.CreateTeam;
using Efficy.Application.Teams.Commands.DeleteTeam;
using Efficy.Application.Teams.Queries.Common;
using Efficy.Application.Teams.Queries.GetAllTeams;
using Efficy.Application.Teams.Queries.GetTeamsTotalSteps;
using Efficy.Application.Teams.Queries.GetTeamTotalSteps;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EfficyTask.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly ISender _sender;

    public TeamsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Lists all Teams
    /// </summary>
    /// <response code="200">Contains List of Teams</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TeamDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTeams(CancellationToken cancellationToken)
    {
        var teams = await _sender.Send(new GetAllTeamsQuery(), cancellationToken);
        return Ok(teams);
    }

    /// <summary>
    /// Lists all Teams and their total step counts
    /// </summary>
    /// <remarks>
    /// You can use this endpoint to compare one team with others
    /// </remarks>
    /// <response code="200">Contains List of Teams and their step counts</response>
    [HttpGet("steps")]
    [ProducesResponseType(typeof(IEnumerable<TeamWithTotalStepsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeamsTotalSteps()
    {
        var teams = await _sender.Send(new GetTeamsTotalStepsQuery());
        return Ok(teams);
    }

    /// <summary>
    /// Gets information about specific Team and its total steps count
    /// </summary>
    /// <remarks>
    /// You can use this endpoint to see how much specific team have walked in total
    /// </remarks>
    /// <param name="teamId">Id of the Team we want to get information for</param>
    /// <response code="404">Team was not found</response>
    /// <response code="200">Contains information about the Team including total steps count</response>
    [HttpGet("steps/{teamId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TeamWithTotalStepsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeamTotalSteps(int teamId)
    {
        var team = await _sender.Send(new GetTeamTotalStepsQuery(teamId));
        return Ok(team);
    }

    /// <summary>
    /// Creates a Team
    /// </summary>
    /// <remarks>
    /// All the parameters in the request body are required.
    ///
    /// **NOTE:** Title must be unique.
    /// </remarks>
    /// <response code="400">Input is invalid. Contains validation errors</response>
    /// <response code="201">Contains the ID of the newly created Team</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTeam(CreateTeamCommand request)
    {
        var teamId = await _sender.Send(request);
        return CreatedAtAction(nameof(GetTeamTotalSteps), new { id = teamId }, teamId);
    }

    /// <summary>
    /// Deletes the Team and all its Counters
    /// </summary>
    /// <remarks>
    /// **NOTE:** Deleting the Team will also delete all its Counters!
    /// </remarks>
    /// <param name="id">Id of the Team to delete</param>
    /// <response code="404">Team was not found</response>
    /// <response code="204">Team was successfully deleted</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        await _sender.Send(new DeleteTeamCommand(id));
        return NoContent();
    }
}