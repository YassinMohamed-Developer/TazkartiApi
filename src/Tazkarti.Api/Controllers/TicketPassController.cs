using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Features.Ticket.Command;
using Tazkarti.Application.Features.Ticket.Query;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class TicketPassController : ControllerBase
	{
		private readonly IMediator _mediator;

		public TicketPassController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAllTickets()
		{
			var UserId = User.FindFirst("UserId")?.Value;

			var result = await _mediator.Send(new GetAllTicketQuery(UserId));
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpGet("{TicketPassId}")]
		public async Task<ActionResult> GetTicket(int TicketPassId)
		{
			var UserId = User.FindFirst("UserId")?.Value;

			var result = await _mediator.Send(new GetTicketQuery(UserId, TicketPassId));
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpPost("{TicketPassId}")]
		public async Task<ActionResult> Verify(int TicketPassId)
		{
			var UserId = User.FindFirst("UserId")?.Value;

			var result = await _mediator.Send(new VerifyTicketByQrCodeCommand(UserId, TicketPassId));
			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
