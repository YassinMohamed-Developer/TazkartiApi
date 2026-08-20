using Microsoft.Extensions.Logging;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.ImplmentationService
{
	public class ProfileService : IProfileService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<ProfileService> _logger;

		public ProfileService(IUnitOfWork unitOfWork,ILogger<ProfileService> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<BaseResult<ProfileDto>> GetProfileAsync(string userId)
		{
			var userdata = await _unitOfWork.Repository<AppUser>().FindByIdAsync(x => x.Id == userId);

			if (userdata == null)
			{
				_logger.LogError($"User with ID {userId} not found.");
				return new BaseResult<ProfileDto>() { Message = "User not found", IsSuccess = false, StatusCode = (int)HttpStatusCode.NotFound };
			}

			var ProfileDto = new ProfileDto
			{
				Id = userdata.Id,
				FanId = userdata.FanId,
				FullName = userdata.FullName,
				Email = userdata.Email,
				PhoneNumber = userdata.PhoneNumber,
				DateOfBirth = userdata.DateOfBirth,
				Gender = userdata.Gender,
				Governorate = userdata.Governorate,
				Nationality = userdata.Nationality,
				AvatarUrl = userdata.AvatarUrl,
				LoyaltyTier = userdata.LoyaltyTier,
				AttendancePoints = userdata.AttendancePoints,
			};

			return new BaseResult<ProfileDto>() { Message = "Profile retrieved successfully",
				IsSuccess = true, Data = ProfileDto };
		}
	}
}
