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
	public record GetTicketQuery(string UserId,int TicketPassId) : IRequest<BaseResult<TicketDto>>;
	public class GetTicketHandler : IRequestHandler<GetTicketQuery, BaseResult<TicketDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetTicketHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResult<TicketDto>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
		{
			var ticketPass = await _unitOfWork.Repository<TicketPass>()
	   .FindByIdAsync(x => x.BookingOrder.UserId == request.UserId && x.Id == request.TicketPassId);
			if (ticketPass == null)
			{
				return new BaseResult<TicketDto>
				{
					IsSuccess = false,
					Message = "No Ticket Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}
			var ticketDto = new TicketDto
			{
				BookingOrderId = ticketPass.BookingOrderId,
				CurrentFanId = ticketPass.CurrentFanId,
				Gate = ticketPass.Gate,
				HolderName = ticketPass.HolderName,
				Status = ticketPass.Status,
				Price = ticketPass.Price,
			};
			return new BaseResult<TicketDto>
			{
				IsSuccess = true,
				Data = ticketDto,
				Message = "Ticket retrieved successfully"
			};
		}
	}
}
