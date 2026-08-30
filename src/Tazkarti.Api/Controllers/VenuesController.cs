using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Features.Venues.Query;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	//[Authorize]
	public class VenuesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public VenuesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllVenues()
		{
			var stadiums = await _mediator.Send(new GetAllVenuesQuery());
			if (!stadiums.IsSuccess)
			{
				return BadRequest();
			}

			return Ok(stadiums);
		}
	}
}
