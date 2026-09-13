using MediatR;
using Shared.Helper;
using System.Net;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Features.Ticket.Query
{
	//Need To Remove
	public record GetEntertainmentEventTicketQuery(string? UserId, int TicketEventId) : IRequest<BaseResult<EntertainmentEventTicketDto>>;

	public class GetEntertainmentEventTicketHandler : IRequestHandler<GetEntertainmentEventTicketQuery, BaseResult<EntertainmentEventTicketDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetEntertainmentEventTicketHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<BaseResult<EntertainmentEventTicketDto>> Handle(GetEntertainmentEventTicketQuery request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(request.UserId) || request.TicketEventId <= 0)
			{
				return new BaseResult<EntertainmentEventTicketDto>
				{
					IsSuccess = false,
					Message = "A valid user and ticket ID are required.",
					StatusCode = (int)HttpStatusCode.BadRequest,
				};
			}

			var ticketPass = await _unitOfWork.Repository<TicketPass>()
				.FindByIdAsync(x => x.BookingOrder.UserId == request.UserId
					&& x.Id == request.TicketEventId
					&& x.BookingOrder.BookingType == BookingType.Event
					&& x.BookingOrder.EventId != null,
					include: "BookingOrder.Event.Venue,BookingOrder.Tier");

			if (ticketPass == null || ticketPass.BookingOrder.Event == null || ticketPass.BookingOrder.Tier == null)
			{
				return new BaseResult<EntertainmentEventTicketDto>
				{
					IsSuccess = false,
					Message = "No Entertainment Event Ticket Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			var entertainmentEvent = ticketPass.BookingOrder.Event;
			var tier = ticketPass.BookingOrder.Tier;
			var ticketDto = new EntertainmentEventTicketDto
			{
				TicketPassId = ticketPass.Id,
				BookingOrderId = ticketPass.BookingOrderId,
				CurrentFanId = ticketPass.CurrentFanId,
				HolderName = ticketPass.HolderName,
				Price = ticketPass.Price,
				Gate = ticketPass.Gate,
				Status = ticketPass.Status,
				EventId = entertainmentEvent.Id,
				Title = entertainmentEvent.Title,
				Category = entertainmentEvent.Category,
				Artist = entertainmentEvent.Artist,
				EventDate = entertainmentEvent.EventDate,
				EventTime = entertainmentEvent.EventTime,
				City = entertainmentEvent.City,
				VenueName = entertainmentEvent.Venue?.Name,
				BannerImage = entertainmentEvent.BannerImage,
				IsActive = entertainmentEvent.IsActive,
				TierId = tier.Id,
				TierName = tier.Name,
				Perks = tier.Perks,
			};

			return new BaseResult<EntertainmentEventTicketDto>
			{
				IsSuccess = true,
				Data = ticketDto,
				Message = "Entertainment event ticket retrieved successfully",
				StatusCode = (int)HttpStatusCode.OK,
			};
		}
	}
}
