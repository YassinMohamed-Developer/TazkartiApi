using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Auth.Command
{
	public record LoginCommand(LoginDto LoginDto):IRequest<BaseResult<TokenDto>>;
	public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResult<TokenDto>>
	{
		private readonly ITokenService _tokenService;
		private readonly ILogger<LoginCommandHandler> _logger;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;

		public LoginCommandHandler(ITokenService tokenService, ILogger<LoginCommandHandler> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
		{
			_tokenService = tokenService;
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task<BaseResult<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
		{

			var NationalId = await _userManager.FindByIdAsync(request.LoginDto.NationalId);

			if (NationalId == null)
			{
				_logger.LogError("No National Id with the provided ID was found.");
				return new BaseResult<TokenDto>()
				{
					Message = ValidationError.AuthError.InvalidNationalId,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest
				};
			}

			var Result = await _signInManager.CheckPasswordSignInAsync(NationalId, request.LoginDto.Password, false);

			if (!Result.Succeeded)
			{
				_logger.LogError("Invalid credentials for the provided National ID.");
				return new BaseResult<TokenDto>()
				{
					Message = ValidationError.AuthError.InvalidCredentials + request.LoginDto.NationalId,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest
				};
			}


			var Token = new TokenDto
			{
				TokenType = "Bearer",
				Token = await _tokenService.GenerateToken(NationalId),
			};

			return new BaseResult<TokenDto>()
			{
				Message = ValidationError.AuthError.LoginSucceeded,
				IsSuccess = true,
				Data = Token,
				StatusCode = (int)HttpStatusCode.OK
			};
		}
	}
}