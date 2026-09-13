using MediatR;
using Shared.Helper;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Ticket.Query;

public record GetAllTicketQuery(string? UserId) : IRequest<BaseResult<IReadOnlyList<TicketDto>>>;

public class GetAllTicketsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllTicketQuery, BaseResult<IReadOnlyList<TicketDto>>>
{
    public async Task<BaseResult<IReadOnlyList<TicketDto>>> Handle(GetAllTicketQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return new BaseResult<IReadOnlyList<TicketDto>> 
            { IsSuccess = false,
                StatusCode = 401,
                Message = "A valid user is required." 
            };

        var tickets = await unitOfWork.Repository<TicketPass>().GetAllWithIdAsync(
            x => x.BookingOrder.UserId == request.UserId, include: TicketResponseMapper.Includes);
        var data = new List<TicketDto>();
        foreach (var ticket in tickets)
        {
            if (!TicketResponseMapper.TryMap(ticket, out var dto))
                return new BaseResult<IReadOnlyList<TicketDto>> 
                { IsSuccess = false,
                    StatusCode = 409,
                    Message = $"Ticket {ticket.Id} has missing or inconsistent booking details." 
                };
            data.Add(dto!);
        }
        return new BaseResult<IReadOnlyList<TicketDto>> 
        { Data = data,
            StatusCode = 200,
            Message = "Tickets retrieved successfully" 
        };
    }
}
