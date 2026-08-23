using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Auth.Command
{
	public record RegisterCommand(RegisterDto RegisterDto) : IRequest<BaseResult<string>>;
	public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResult<string>>
	{
		private readonly ILogger<LoginCommandHandler> _logger;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		public RegisterCommandHandler(ILogger<LoginCommandHandler> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task<BaseResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
		{
			var NationalId = await _userManager.FindByIdAsync(request.RegisterDto.NationalId);

			if (NationalId != null)
			{
				return new BaseResult<string>()
				{
					Message = ValidationError.AuthError.HaveSameNationalId,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest
				};
			}

			var UserName = await _userManager.FindByNameAsync(request.RegisterDto.NationalName);

			if (UserName != null)
			{
				return new BaseResult<string>()
				{
					Message = ValidationError.AuthError.UserNameAlreadyExists,
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest
				};
			}

			var Email = await _userManager.FindByEmailAsync(request.RegisterDto.Email);
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
				UserName = request.RegisterDto.NationalName.Trim().Replace(" ", ""),
				Email = request.RegisterDto.Email,
				FullName = request.RegisterDto.NationalName.Trim().Replace(" ", ""),
				DateOfBirth = request.RegisterDto.DateOfBirth,
				Gender = request.RegisterDto.Gender,
				Governorate = request.RegisterDto.Governorate,
				PhoneNumber = request.RegisterDto.PhoneNumber,
				Id = request.RegisterDto.NationalId,
				FanId = GenerateFanId(),
			};

			var creationgUser = await _userManager.CreateAsync(User, request.RegisterDto.Password);

			if (creationgUser.Succeeded)
			{
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
