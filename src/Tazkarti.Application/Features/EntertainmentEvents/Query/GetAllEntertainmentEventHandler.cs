using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.EntertainmentEvents.Query
{
	public record GetAllEntertainmentEventQuery() : IRequest<BaseResult<IReadOnlyList<EntertainmentEventDto>>>;

	public class GetAllEntertainmentEventHandler : IRequestHandler<GetAllEntertainmentEventQuery, BaseResult<IReadOnlyList<EntertainmentEventDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GetAllEntertainmentEventHandler> _logger;

		public GetAllEntertainmentEventHandler(IUnitOfWork unitOfWork, ILogger<GetAllEntertainmentEventHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<BaseResult<IReadOnlyList<EntertainmentEventDto>>> Handle(GetAllEntertainmentEventQuery request, CancellationToken cancellationToken)
		{
			var events = await _unitOfWork.Repository<EntertainmentEvent>().GetAllAsync(include:"Venue,TicketTiers");

			if (events == null || events.Count == 0)
			{
				_logger.LogError("No entertainment events found in the database.");
				return new BaseResult<IReadOnlyList<EntertainmentEventDto>>()
				{
					IsSuccess = false,
					Message = "No Entertainment Events Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			var eventDtos = events.Select(e => new EntertainmentEventDto
			{
				Id = e.Id,
				Title = e.Title,
				Category = e.Category,
				Tag = e.Tag,
				Artist = e.Artist,
				EventDate = e.EventDate,
				EventTime = e.EventTime,
				VenueId = e.VenueId,
				VenueName = e.Venue?.Name,
				City = e.City,
				MinPrice = e.MinPrice,
				BannerImage = e.BannerImage,
				Description = e.Description,
				IsActive = e.IsActive,
				TierId = e.TicketTiers.Select(x => x.Id).ToList(),
				NameOfTicketTier = e.TicketTiers.Select(t => t.Name).ToList(),
				Price = e.TicketTiers.Select(t => t.Price).ToList(),
				Perks = e.TicketTiers.Select(t => t.Perks).ToList(),
			}).ToList();

			return new BaseResult<IReadOnlyList<EntertainmentEventDto>>
			{
				IsSuccess = true,
				Message = "Entertainment events retrieved successfully",
				StatusCode = (int)HttpStatusCode.OK,
				Data = eventDtos
			};
		}
	}
}
