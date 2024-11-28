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
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TeamDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTeams()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        return Ok(teams);
    }

    [HttpGet("steps")]
    [ProducesResponseType(typeof(IEnumerable<TeamWithTotalStepsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeamsTotalSteps()
    {
        var teams = await _sender.Send(new GetTeamsTotalStepsQuery());
        return Ok(teams);
    }

    [HttpGet("steps/{teamId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(TeamWithTotalStepsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeamTotalSteps(int teamId)
    {
        var team = await _sender.Send(new GetTeamTotalStepsQuery(teamId));
        return Ok(team);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTeam(CreateTeamCommand request)
    {
        var teamId = await _sender.Send(request);
        return CreatedAtAction(nameof(GetTeamTotalSteps), new { id = teamId }, teamId);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        await _sender.Send(new DeleteTeamCommand(id));
        return NoContent();
    }
}