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

namespace Tazkarti.Infrastructure.ImplmentationService
{
	public class AuthService : IAuthService
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ILogger<AuthService> _logger;

		public AuthService(ITokenService tokenService,
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			ILogger<AuthService> logger)
		{
			_tokenService = tokenService;
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
		}
		public async Task<BaseResult<TokenDto>> LoginAsync(LoginDto loginDto)
		{
			var NationalId = await _userManager.FindByIdAsync(loginDto.NationalId);

			if(NationalId == null)
			{
				_logger.LogError("No National Id with the provided ID was found.");
				return new BaseResult<TokenDto>() { Message = ValidationError.AuthError.InvalidNationalId,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var Result = await _signInManager.CheckPasswordSignInAsync(NationalId, loginDto.Password, false);

			if (!Result.Succeeded)
			{
				_logger.LogError("Invalid credentials for the provided National ID.");
				return new BaseResult<TokenDto>()
				{
					Message = ValidationError.AuthError.InvalidCredentials + loginDto.NationalId,
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
		public async Task<BaseResult<string>> RegisterAsync(RegisterDto registerDto)
		{
			var NationalId = await _userManager.FindByIdAsync(registerDto.NationalId);

			if(NationalId != null)
			{
				return new BaseResult<string>() { Message = ValidationError.AuthError.HaveSameNationalId,
					IsSuccess = false,StatusCode = (int)HttpStatusCode.BadRequest };
			}

			var UserName = await _userManager.FindByNameAsync(registerDto.NationalName);

			if(UserName != null)
			{
				return new BaseResult<string>() {Message = ValidationError.AuthError.UserNameAlreadyExists,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest
				};
			}

			var Email = await _userManager.FindByEmailAsync(registerDto.Email);
			if (Email != null)
			{
				return new BaseResult<string>()
				{
					Message = ValidationError.AuthError.EmailAlreadyExists,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest
				};
			}

			var User = new AppUser
			{
				UserName = registerDto.NationalName.Trim().Replace(" ", ""),
				Email = registerDto.Email,
				FullName = registerDto.NationalName.Trim().Replace(" ", ""),
				DateOfBirth = registerDto.DateOfBirth,
				Gender = registerDto.Gender,
				Governorate = registerDto.Governorate,
				PhoneNumber = registerDto.PhoneNumber,
				Id = registerDto.NationalId,
				FanId = GenerateFanId(),
			};

			var creationgUser = await _userManager.CreateAsync(User, registerDto.Password);

			if(creationgUser.Succeeded) {
				return new BaseResult<string>()
				{
					Message = ValidationError.AuthError.RegistrationSucceeded,
					IsSuccess = true,
					StatusCode = (int)HttpStatusCode.OK
				};
			}
			return new BaseResult<string>()
			{
				Message = ValidationError.AuthError.RegistrationFailed,
				IsSuccess = false,
				StatusCode = (int)HttpStatusCode.BadRequest
			};
		}

		private string GenerateFanId()
		{
			var random = new Random();
			return "Fan Id : TZK-" + random.Next(100000, 999999).ToString();
		}
	}
}
