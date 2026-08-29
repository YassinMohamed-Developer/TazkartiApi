using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Features.Clubs.Query;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ClubController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ClubController(IMediator mediator) 
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAllClubs()
		{
			var result = await _mediator.Send(new GetAllClubsQueryRequest());
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
