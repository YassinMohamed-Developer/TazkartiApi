using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Features.Matches.Query;
using Tazkarti.Application.Features.Venues.Query;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MatchesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public MatchesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllMatches()
		{
			var stadiums = await _mediator.Send(new GetAllMatchesQuery());
			if (!stadiums.IsSuccess)
			{
				return BadRequest();
			}

			return Ok(stadiums);
		}
	}
}
