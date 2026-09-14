using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Matches.Query
{
	public record GetAllMatchesQuery() : IRequest<BaseResult<IReadOnlyList<AllMatchesResponseDto>>>;
	public class GetAllMatchesHandler : IRequestHandler<GetAllMatchesQuery, BaseResult<IReadOnlyList<AllMatchesResponseDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GetAllMatchesHandler> _logger;

		public GetAllMatchesHandler(IUnitOfWork unitOfWork, ILogger<GetAllMatchesHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<BaseResult<IReadOnlyList<AllMatchesResponseDto>>> Handle(GetAllMatchesQuery request, CancellationToken cancellationToken)
		{
			var matches = await _unitOfWork.Repository<FootballMatch>()
				.GetAllWithProjectionAsync(selector: m => new 
				{
					Id = m.Id,
					Title = m.Title,
					Competition = m.Competition,
					Round = m.Round,
					MatchDate = m.MatchDate,
					KickoffTime = m.KickoffTime,
					GateOpenTime = m.GateOpenTime,
					AvailabilityPercent = m.AvailabilityPercent,
					AvailabilityStatus = m.AvailabilityStatus,
					BannerImage = m.BannerImage,
					City = m.City,
					MinPrice = m.MinPrice,
					VenueId = m.VenueId,
					HomeTeamName = m.HomeTeam.Name,
					AwayTeamName = m.AwayTeam.Name,
					VenueName = m.Venue.Name,
				}, include: "HomeTeam,AwayTeam,Venue");

			if (matches == null)
			{
				_logger.LogError("No Matches found in the database.");
				return new BaseResult<IReadOnlyList<AllMatchesResponseDto>>()
				{
					IsSuccess = false,
					Message = "No Matches Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			var MatchDto = matches.Select(m => new AllMatchesResponseDto
			{
				Id = m.Id,
				Title = m.Title,
				Competition = m.Competition,
				Round = m.Round,
				HomeTeamName = m.HomeTeamName,
				AwayTeamName = m.AwayTeamName,
				MatchDate = m.MatchDate,
				KickoffTime = m.KickoffTime,
				GateOpenTime = m.GateOpenTime,
				AvailabilityPercent = m.AvailabilityPercent,
				AvailabilityStatus = m.AvailabilityStatus,
				BannerImage = m.BannerImage,
				City = m.City,
				MinPrice = m.MinPrice,
				VenueName = m.VenueName,
				VenueId = m.VenueId,
			}).ToList();

			return new BaseResult<IReadOnlyList<AllMatchesResponseDto>>
			{
				Message = "Data Retireve Succesffuly",
				IsSuccess = true,
				StatusCode = (int)HttpStatusCode.OK,
				Data = MatchDto
			};
		}
	}
}
