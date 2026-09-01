using MediatR;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Features.Ticket.Command
{
	public record VerifyTicketByQrCodeCommand(string UserId,int TicketPassId) : IRequest<BaseResult<TicketDto>>;
	public class VerifyTicketByQrCodeHandler : IRequestHandler<VerifyTicketByQrCodeCommand, BaseResult<TicketDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public VerifyTicketByQrCodeHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResult<TicketDto>> Handle(VerifyTicketByQrCodeCommand request, CancellationToken cancellationToken)
		{
			var ticketPass = await _unitOfWork.Repository<TicketPass>()
			.FindAndProjectAsync(x => x.BookingOrder.UserId == request.UserId
			&& x.Id == request.TicketPassId, x => new
			{
				x.IsActive,
				x.Id,
			});
			if (ticketPass == null)
			{
				return new BaseResult<TicketDto>
				{
					IsSuccess = false,
					Message = "No Ticket Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			if (ticketPass.IsActive == false)
			{
				var TicketPass = new TicketPass
				{
					Id = ticketPass.Id,
					Status =TicketStatus.Cancelled,
				};

				 _unitOfWork.Repository<TicketPass>().UpdateProperty(TicketPass, x => x.Status);

				await _unitOfWork.SaveChangesAsync();
				return new BaseResult<TicketDto>
				{
					IsSuccess = true,
					Message = "Ticket is DENIED",
					StatusCode = (int)HttpStatusCode.OK,
				};
			}
			else
			{
				var TicketPass = new TicketPass
				{
					Id = ticketPass.Id,
					Status = TicketStatus.Attended,
				};

				_unitOfWork.Repository<TicketPass>().UpdateProperty(TicketPass, x => x.Status);
				await _unitOfWork.SaveChangesAsync();
				return new BaseResult<TicketDto>
				{
					IsSuccess = true,
					Message = "Ticket Is APPROVED ",
					StatusCode = (int)HttpStatusCode.OK,
				};
				
			}	
		}
	}
}
