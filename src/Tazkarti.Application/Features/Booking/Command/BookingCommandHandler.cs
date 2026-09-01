using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Features.Booking.Command
{
	public record BookingCommand(BookingDto BookingDto, string userId) : IRequest<BaseResult<string>>;

	public class BookingCommandHandler : IRequestHandler<BookingCommand, BaseResult<string>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<BookingCommandHandler> _logger;

		public BookingCommandHandler(IUnitOfWork unitOfWork, ILogger<BookingCommandHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<BaseResult<string>> Handle(BookingCommand request, CancellationToken cancellationToken)
		{

			if (request?.BookingDto == null || string.IsNullOrWhiteSpace(request.userId))
			{
				return new BaseResult<string>
				{
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = "Invalid booking submission."
				};
			}

			if (request.BookingDto.Quantity <= 0 || request.BookingDto.Quantity > 4)
			{
				return new BaseResult<string>
				{
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = "Quantity must be between 1 and 4."
				};
			}

			if (request.BookingDto.MatchId.HasValue)
			{
				var existingMatchBookings = await _unitOfWork.Repository<BookingOrder>()
					.GetAllWithIdAsync(x =>
						x.UserId == request.userId &&
						x.MatchId == request.BookingDto.MatchId.Value &&
						x.Status != BookingStatus.Cancelled);

				int totalBookedSoFar = existingMatchBookings?.Sum(x => x.Quantity) ?? 0;

				if (totalBookedSoFar + request.BookingDto.Quantity > 4)
				{
					int remainingAllowed = Math.Max(0, 4 - totalBookedSoFar);
					return new BaseResult<string>
					{
						IsSuccess = false,
						StatusCode = (int)HttpStatusCode.BadRequest,
						Message = remainingAllowed > 0
							? $"You have already booked {totalBookedSoFar} tickets. You can only book {remainingAllowed} more."
							: "You have already reached the maximum limit of 4 tickets for this match."
					};
				}
			}

			decimal unitPrice = 0;

			if (request.BookingDto.CategoryId.HasValue)
			{
				var category = await _unitOfWork.Repository<MatchTicketCategory>()
					.GetByIdAsync(request.BookingDto.CategoryId.Value);

				if (category == null)
				{
					return new BaseResult<string>
					{
						IsSuccess = false,
						StatusCode = (int)HttpStatusCode.NotFound,
						Message = "Selected ticket category was not found."
					};
				}

				unitPrice = category.Price;

				if(category.Available < request.BookingDto.Quantity)
				{
					return new BaseResult<string>
					{
						IsSuccess = false,
						StatusCode = (int)HttpStatusCode.Conflict,
						Message = $"Only {category.Available} tickets available for this category."
					};

				}
				if(category.MatchId != request.BookingDto.MatchId.Value)
				{
					return new BaseResult<string>
					{
						IsSuccess = false,
						StatusCode = (int)HttpStatusCode.BadRequest,
						Message = "Selected category does not belong to the specified match."
					};
				}
					category.Available -= request.BookingDto.Quantity;
					_unitOfWork.Repository<MatchTicketCategory>().Update(category);
			}
			else if (request.BookingDto.TierId.HasValue)
			{
				var tier = await _unitOfWork.Repository<EventTicketTier>()
					.GetByIdAsync(request.BookingDto.TierId.Value);

				if (tier == null)
				{
					return new BaseResult<string>
					{
						IsSuccess = false,
						StatusCode = (int)HttpStatusCode.NotFound,
						Message = "Selected ticket tier was not found."
					};
				}

				unitPrice = tier.Price;
			}
			else
			{
				return new BaseResult<string>
				{
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = "A valid Category or Tier must be selected."
				};
			}

			var bookingOrder = new BookingOrder
			{
				BookingReference = $"BK-{Guid.NewGuid():N}"[..11].ToUpper(),
				UserId = request.userId,
				MatchId = request.BookingDto.MatchId,
				EventId = request.BookingDto.EventId,
				CategoryId = request.BookingDto.CategoryId,
				TierId = request.BookingDto.TierId,
				BookingType = request.BookingDto.BookingType,
				PaymentMethod = request.BookingDto.PaymentMethod,
				Block = request.BookingDto.Block,
				Quantity = request.BookingDto.Quantity,
				Gate = request.BookingDto.Gate,
				VenueId = request.BookingDto.VenueId,
				City = request.BookingDto.City ?? string.Empty,
				TotalAmount = request.BookingDto.Quantity * unitPrice,
			};

			await _unitOfWork.Repository<BookingOrder>().AddAsync(bookingOrder);
			bookingOrder.Status = BookingStatus.Confirmed;

			var userid = await _unitOfWork.Repository<AppUser>()
				.FindAndProjectAsync(x => x.Id == request.userId,x => new
				{
					x.FanId,
					x.FullName
				});

			var match = await _unitOfWork.Repository<FootballMatch>()
					.FindAndProjectAsync(
						x => x.Id == request.BookingDto.MatchId,
						x => new
						{
							x.Title,
							x.Competition,
							x.Round,
							x.IsActive,
							HomeTeamName = x.HomeTeam.Name,
							AwayTeamName = x.AwayTeam.Name
						});

			if (userid == null)
			{
				return new BaseResult<string>
				{
					IsSuccess = false,
					StatusCode = (int)HttpStatusCode.NotFound,
					Message = "User not found."
				};
			}
			for (int i = 1; i <= request.BookingDto.Quantity; i++)
			{

				var ticketpass = new TicketPass
				{
					BookingOrderId = bookingOrder.Id,
					CurrentFanId = userid.FanId,
					OriginalFanId = userid.FanId,
					HolderName = userid.FullName,
					Price = bookingOrder.TotalAmount,
					Gate = bookingOrder.Gate,
					Status = TicketStatus.Confirmed,
					Competition = match?.Competition,
					HomeTeam = match?.HomeTeamName,
					AwayTeam = match?.AwayTeamName,
					Title = match != null ? $"{match.HomeTeamName} vs {match.AwayTeamName}" : null,
					Round = match?.Round,
					IsActive = match?.IsActive,
				};

				bookingOrder.Tickets.Add(ticketpass);
			}

			await _unitOfWork.SaveChangesAsync();

			_logger.LogInformation("Booking order {Reference} created for user {UserId}", bookingOrder.BookingReference, request.userId);

			return new BaseResult<string>
			{
				IsSuccess = true,
				StatusCode = (int)HttpStatusCode.OK,
				Message = $"Booking confirmed successfully. Reference: {bookingOrder.BookingReference}"
			};
		}
	}
}

