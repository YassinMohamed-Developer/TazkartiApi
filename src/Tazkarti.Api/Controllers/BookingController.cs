using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Features.Booking.Command;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class BookingController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BookingController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult> Booking(BookingDto bookingDto)
		{
			var Userid = User.FindFirst("UserId")?.Value.ToString();

			var result = await _mediator.Send(new BookingCommand(bookingDto,Userid));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
