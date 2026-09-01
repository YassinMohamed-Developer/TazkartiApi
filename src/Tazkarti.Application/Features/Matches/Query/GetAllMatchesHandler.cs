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
	public record GetAllMatchesQuery() : IRequest<BaseResult<IReadOnlyList<MatchDto>>>;
	public class GetAllMatchesHandler : IRequestHandler<GetAllMatchesQuery, BaseResult<IReadOnlyList<MatchDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GetAllMatchesHandler> _logger;

		public GetAllMatchesHandler(IUnitOfWork unitOfWork, ILogger<GetAllMatchesHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<BaseResult<IReadOnlyList<MatchDto>>> Handle(GetAllMatchesQuery request, CancellationToken cancellationToken)
		{
			var matches = await _unitOfWork.Repository<FootballMatch>().GetAllAsync(include: "HomeTeam,AwayTeam,Venue,TicketCategories");

			if (matches == null)
			{
				_logger.LogError("No Stadiums found in the database.");
				return new BaseResult<IReadOnlyList<MatchDto>>()
				{
					IsSuccess = false,
					Message = "No Matches Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			var MatchDto = matches.Select(m => new MatchDto
			{
				Title = m.Title,
				Competition = m.Competition,
				Round = m.Round,
				HomeTeamName = m.HomeTeam.Name,
				AwayTeamName = m.AwayTeam.Name,
				MatchDate = m.MatchDate,
				KickoffTime = m.KickoffTime,
				GateOpenTime = m.GateOpenTime,
				AvailabilityPercent = m.AvailabilityPercent,
				AvailabilityStatus = m.AvailabilityStatus,
				BannerImage = m.BannerImage,
				City = m.City,
				MinPrice = m.MinPrice,
				VenueName = m.Venue.Name,
				NameOfCategoryMatch = m.TicketCategories.Select(x => x.Name).ToList(),
				Available = m.TicketCategories.Select(a => a.Available).ToList(),
				Price = m.TicketCategories.Select(p => p.Price).ToList(),
				GateAllocation = m.TicketCategories.Select(G => G.GateAllocation).ToList(),
			}).ToList();

			return new BaseResult<IReadOnlyList<MatchDto>>
			{
				Message = "Data Retireve Succesffuly",
				IsSuccess = true,
				StatusCode = (int)HttpStatusCode.OK,
				Data = MatchDto
			};
		}
	}
}
