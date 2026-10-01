using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using Shared.Helper.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Ticket.Query
{
	public record GenerateTicketPdfQuery(int TicketId) : IRequest<BaseResult<byte[]>>;
	public class GenerateTicketPdfQueryHandler : IRequestHandler<GenerateTicketPdfQuery, BaseResult<byte[]>>
	{
		private readonly ITazkaraPdf _tazkaraPdf;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GenerateTicketPdfQueryHandler> _logger;

		public GenerateTicketPdfQueryHandler(ITazkaraPdf tazkaraPdf,
			IUnitOfWork unitOfWork,ILogger<GenerateTicketPdfQueryHandler> logger)
		{
			_tazkaraPdf = tazkaraPdf;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<BaseResult<byte[]>> Handle(GenerateTicketPdfQuery request, CancellationToken cancellationToken)
		{

			if (request.TicketId <= 0)
			{
				return new BaseResult<byte[]>
				{
					IsSuccess = false,
					Message = "Invalid Ticket Id",
					Errors = new List<string> { "Invalid Ticket Id" },
					StatusCode = 400
				};
			}


			var TazkaraInfo = await _unitOfWork.Repository<TicketPass>().FindAndProjectAsync(x => x.Id == request.TicketId, include:"BookingOrder",x => new MatchTazkaraDto
			{
				BookingOrderId = x.BookingOrderId,
				BookingReference = x.BookingOrder.BookingReference,
				FanId = x.CurrentFanId,
				AwayTeamName = x.AwayTeam,
				HolderName = x.HolderName,
				CompetitionName = x.Competition,
				HomeTeamName = x.HomeTeam,
				Price = x.Price,
				Gate = x.Gate,
				IsActive = x.IsActive,
				Round = x.Round,
				Title = x.Title,
			});

			if(TazkaraInfo is null)
			{
				return new BaseResult<byte[]>
				{
					IsSuccess = false,
					Message = "Ticket not found",
					Errors = new List<string> { "Ticket not found" },
					StatusCode = 404
				};
			}

			var pdfData = _tazkaraPdf.GeneratePdf(TazkaraInfo);

			_logger.LogInformation("Generating Tazkara PDF for TicketId {TicketId}",
				request.TicketId);


			return new BaseResult<byte[]>
			{
				IsSuccess = true,
				Message = "PDF generated successfully",
				Data = pdfData,
				StatusCode = 200
			};
		}
	}
}
