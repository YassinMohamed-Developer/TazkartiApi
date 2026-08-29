using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Features.Profile.Query;

public record GetProfileQuery(string UserId) : IRequest<BaseResult<ProfileDto>>;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, BaseResult<ProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProfileQueryHandler> _logger;

    public GetProfileQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProfileQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResult<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<AppUser>().FindByIdAsync(x => x.Id == request.UserId
        ,include: "FavouriteClub");

        if (user is null)
        {
            _logger.LogWarning("Profile requested for unknown user {UserId}.", request.UserId);
            return new BaseResult<ProfileDto>
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        var profile = new ProfileDto
        {
            Id = user.Id,
            FanId = user.FanId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            Governorate = user.Governorate,
            Nationality = user.Nationality,
            AvatarUrl = user.AvatarUrl,
            LoyaltyTier = user.LoyaltyTier,
            AttendancePoints = user.AttendancePoints,
            FavouriteClubName = user.FavouriteClub?.Name
        };

        return new BaseResult<ProfileDto>
        {
            Message = "Profile retrieved successfully",
            IsSuccess = true,
            Data = profile,
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
