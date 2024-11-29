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

        /// <summary>
        /// Lists all counters for specific Team
        /// </summary>
        /// <remarks>
        /// You can use this endpoint to see how much each team member have walked
        /// </remarks>
        /// <param name="teamId">Id of the Team we want to get counters for</param>
        /// <response code="404">Team was not found</response>
        /// <response code="200">Contains all counters for specific Team</response>
        [HttpGet("team/{teamId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<CounterForTeamDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountersForTeam(int teamId)
        {
            var countersForTeam = await _sender.Send(new GetAllCountersForTeamQuery(teamId));
            return Ok(countersForTeam);
        }

        /// <summary>
        /// Gets information about specific counter
        /// </summary>
        /// <param name="id">Id of the Counter</param>
        /// <response code="404">Counter was not found</response>
        /// <response code="200">Contains information about the counter</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CounterDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var counter = await _sender.Send(new GetCounterByIdQuery(id));
            return Ok(counter);
        }

        /// <summary>
        /// Creates a Counter and assigns it to the specified Team
        /// </summary>
        /// <remarks>
        /// All the parameters in the request body are required.
        /// 
        /// **NOTE: Name must be unique within the Team**.
        /// So you can have a counter with the same name in different Teams, **but not within the same Team**.
        /// </remarks>
        /// <response code="400">Input is invalid. Contains validation errors</response>
        /// <response code="201">Contains the ID of the newly created Counter</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCounter(CreateCounterCommand request)
        {
            var id = await _sender.Send(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Increments the value of the specified Counter
        /// </summary>
        /// <remarks>
        /// You can use this endpoint to count steps for the Team the Counter belongs to.
        ///
        /// All the parameters in the request body are required.
        ///
        /// **NOTE:** 'IncrementValue' must be in the range from 1 to 100 inclusive.
        /// </remarks>
        /// <response code="400">Input is invalid. Contains validation errors</response>
        /// <response code="404">Counter was not found</response>
        /// <response code="200">Contains the new value of the Counter</response>
        [HttpPut("increment")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> IncrementCounter(IncrementCounterCommand request)
        {
            var newValue = await _sender.Send(request);
            return Ok(newValue);
        }

        /// <summary>
        /// Deletes the Counter
        /// </summary>
        /// <param name="counterId">Id of the Counter to delete</param>
        /// <response code="404">Counter was not found</response>
        /// <response code="204">Counter was successfully deleted</response>
        [HttpDelete("{counterId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCounter(int counterId)
        {
            await _sender.Send(new DeleteCounterCommand(counterId));
            return NoContent();
        }
    }
}
