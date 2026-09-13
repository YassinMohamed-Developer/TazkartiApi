using MediatR;
using Shared.Helper;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Ticket.Query;

public record GetTicketQuery(string? UserId, int TicketPassId) : IRequest<BaseResult<TicketDto>>;

public class GetTicketHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTicketQuery, BaseResult<TicketDto>>
{
    public async Task<BaseResult<TicketDto>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return new BaseResult<TicketDto> { 
                IsSuccess = false,
                StatusCode = 401,
                Message = "A valid user is required." 
            };
        if (request.TicketPassId <= 0)
            return new BaseResult<TicketDto> 
            { IsSuccess = false,
                StatusCode = 400,
                Message = "A valid ticket ID is required." 
            };

        var ticket = await unitOfWork.Repository<TicketPass>().FindByIdAsync(
            x => x.BookingOrder.UserId == request.UserId && x.Id == request.TicketPassId,
            include: TicketResponseMapper.Includes);
        if (ticket == null)
            return new BaseResult<TicketDto> 
            { IsSuccess = false,
                StatusCode = 404,
                Message = "No Ticket Found" 
            };
        if (!TicketResponseMapper.TryMap(ticket, out var dto))
            return new BaseResult<TicketDto> 
            { IsSuccess = false,
                StatusCode = 409,
                Message = "Ticket has missing or inconsistent booking details." 
            };
        return new BaseResult<TicketDto> 
        {   Data = dto!,
            StatusCode = 200,
            Message = "Ticket retrieved successfully" 
        };
    }
}
