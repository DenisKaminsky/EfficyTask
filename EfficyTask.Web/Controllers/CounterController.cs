using Efficy.Application.Counters.Commands.CreateCounter;
using Efficy.Application.Counters.Commands.DeleteCounter;
using Efficy.Application.Counters.Commands.IncrementCounter;
using Efficy.Application.Counters.Queries.GetAllCountersForTeam;
using Efficy.Application.Counters.Queries.GetCounterById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EfficyTask.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly ISender _sender;

        public CounterController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("team/{teamId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountersForTeam(int teamId)
        {
            var countersForTeam = await _sender.Send(new GetAllCountersForTeamQuery(teamId));
            return Ok(countersForTeam);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var counter = await _sender.Send(new GetCounterByIdQuery(id));
            return Ok(counter);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCounter(CreateCounterCommand request)
        {
            var id = await _sender.Send(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("increment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IncrementCounter(IncrementCounterCommand request)
        {
            var newValue = await _sender.Send(request);
            return Ok(newValue);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCounter(int id)
        {
            await _sender.Send(new DeleteCounterCommand(id));
            return NoContent();
        }
    }
}
