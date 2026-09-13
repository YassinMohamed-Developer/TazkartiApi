using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Features.EntertainmentEvents.Query;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class EntertainmentEventsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public EntertainmentEventsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllEntertainmentEvent()
		{
			var events = await _mediator.Send(new GetAllEntertainmentEventQuery());
			if (!events.IsSuccess)
			{
				return BadRequest();
			}

			return Ok(events);
		}
	}
}
