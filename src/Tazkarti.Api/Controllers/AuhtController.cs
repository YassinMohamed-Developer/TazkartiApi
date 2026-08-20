using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Helper;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;

namespace Tazkarti.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost]
		public async Task<ActionResult<BaseResult<TokenDto>>> SignIn(LoginDto loginDto)
		{
			var result = await _authService.LoginAsync(loginDto);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		[HttpPost]

		public async Task<ActionResult<BaseResult<string>>> Register(RegisterDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

	}
}
