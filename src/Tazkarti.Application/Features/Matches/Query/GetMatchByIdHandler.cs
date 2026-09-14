using MediatR;
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

namespace Tazkarti.Application.Features.Matches.Query
{
	public record GetMatchByIdQuery(int MatchId) : IRequest<BaseResult<MatchResponseDto>>;
	public class GetMatchByIdHandler : IRequestHandler<GetMatchByIdQuery, BaseResult<MatchResponseDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GetMatchByIdQuery> _logger;

		public GetMatchByIdHandler(IUnitOfWork unitOfWork,ILogger<GetMatchByIdQuery> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<BaseResult<MatchResponseDto>> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
		{
			var Match = await _unitOfWork.Repository<FootballMatch>()
				.FindAndProjectAsync(x => x.Id == request.MatchId, include: "HomeTeam,AwayTeam,Venue,TicketCategories", x => new
				{
					Id = x.Id,
					Title = x.Title,
					Competition = x.Competition,
					Round = x.Round,
					City = x.City,
					MatchDate = x.MatchDate,
					KickoffTime = x.KickoffTime,
					GateOpenTime = x.GateOpenTime,
					AvailabilityStatus = x.AvailabilityStatus,
					AvailabilityPercent = x.AvailabilityPercent,
					MinPrice = x.MinPrice,
					BannerImage = x.BannerImage,
					HomeTeamName = x.HomeTeam.Name,
					AwayTeamName = x.AwayTeam.Name,
					VenueName = x.Venue.Name,
					VenueId = x.VenueId,
					TicketCategories = x.TicketCategories.Select(tc => new TicketCategoryDto
					{
						Id = tc.Id,
						Name = tc.Name,
						Price = tc.Price,
						Available = tc.Available,
						GateAllocation = tc.GateAllocation!
					}).ToList()
				});

			if(Match == null)
			{
				_logger.LogError("No Matches found in the database.");
				return new BaseResult<MatchResponseDto>()
				{
					IsSuccess = false,
					Message = "No Matches Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}
			var matchDto = new MatchResponseDto
			{
				Id = Match.Id,
				Title = Match.Title,
				Competition = Match.Competition,
				Round = Match.Round,
				City = Match.City,
				MatchDate = Match.MatchDate,
				KickoffTime = Match.KickoffTime,
				GateOpenTime = Match.GateOpenTime,
				AvailabilityStatus = Match.AvailabilityStatus,
				AvailabilityPercent = Match.AvailabilityPercent,
				MinPrice = Match.MinPrice,
				BannerImage = Match.BannerImage,
				HomeTeamName = Match.HomeTeamName,
				AwayTeamName = Match.AwayTeamName,
				VenueName = Match.VenueName,
				VenueId = Match.VenueId,
				TicketCategories = Match.TicketCategories
			};
			return new BaseResult<MatchResponseDto>()
			{
				IsSuccess = true,
				Message = "Match Found",
				StatusCode = (int)HttpStatusCode.OK,
				Data = matchDto
			};
		}

	}
}
