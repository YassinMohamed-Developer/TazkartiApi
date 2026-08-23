using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Helper;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Features.Auth.Command;
using Tazkarti.Application.Interfaces;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult<BaseResult<TokenDto>>> SignIn(LoginDto loginDto)
		{

			var result = await _mediator.Send(new LoginCommand(loginDto));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpPost]

		public async Task<ActionResult<BaseResult<string>>> Register(RegisterDto registerDto)
		{
			var result = await _mediator.Send(new RegisterCommand(registerDto));

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

	}
}
