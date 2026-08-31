using MediatR;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Ticket.Query
{
	public record GetAllTicketQuery(string UserId) : IRequest<BaseResult<IReadOnlyList<TicketDto>>>;
	public class GetAllTicketsHandler : IRequestHandler<GetAllTicketQuery, BaseResult<IReadOnlyList<TicketDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllTicketsHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<BaseResult<IReadOnlyList<TicketDto>>> Handle(GetAllTicketQuery request, CancellationToken cancellationToken)
		{
			var Tickets = await _unitOfWork.Repository<TicketPass>()
				.GetAllWithIdAsync(x => x.BookingOrder.UserId == request.UserId);
			if (Tickets == null)
			{
				return new BaseResult<IReadOnlyList<TicketDto>>
				{
					IsSuccess = false,
					Message = "No Ticket Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			var ticketDto = Tickets.Select(x => new TicketDto
			{
				BookingOrderId = x.BookingOrderId,
				CurrentFanId = x.CurrentFanId,
				Gate = x.Gate,
				HolderName = x.HolderName,
				Price = x.Price,
			}).ToList();

			return new BaseResult<IReadOnlyList<TicketDto>>
			{
				IsSuccess = true,
				Data = ticketDto,
				Message = "Ticket retrieved successfully"
			};
		}
	}
}
