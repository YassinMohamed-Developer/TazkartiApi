using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Features.ChatBot.Command;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class ChatBotController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ChatBotController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Ask([FromBody] ChatRequest request)
		{

			var UserId = User.FindFirst("UserId")?.Value;

			if (UserId == null)
			{
				return BadRequest("You Must Be Logged In to Ask Questions");
			}

			var result = await _mediator.Send(new ChatBotCommand(request, UserId));

			return Ok(result);
		}
	}
}
