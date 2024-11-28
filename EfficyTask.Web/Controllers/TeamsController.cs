using Efficy.Application.Teams.Commands.CreateTeam;
using Efficy.Application.Teams.Commands.DeleteTeam;
using Efficy.Application.Teams.Queries.GetAllTeams;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var teams = await _sender.Send(new GetAllTeamsQuery());
        return Ok(teams);
    }
    
    [HttpGet("{id}")]
    public string GetById(int id)
    {
        return "value";
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(CreateTeamCommand request)
    {
        var id = await _sender.Send(request);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _sender.Send(new DeleteTeamCommand(id));
        return NoContent();
    }
}