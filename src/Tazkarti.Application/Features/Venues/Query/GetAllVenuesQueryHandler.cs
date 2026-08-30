using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Venues.Query
{

	public record GetAllVenuesQuery() : IRequest<BaseResult<IReadOnlyList<VenuesDto>>>;
	public class GetAllVenuesQueryHandler : IRequestHandler<GetAllVenuesQuery, BaseResult<IReadOnlyList<VenuesDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GetAllVenuesQueryHandler> _logger;

		public GetAllVenuesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllVenuesQueryHandler> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		public async Task<BaseResult<IReadOnlyList<VenuesDto>>> Handle(GetAllVenuesQuery request, CancellationToken cancellationToken)
		{
			var Stadiums = await _unitOfWork.Repository<StadiumVenue>().GetAllAsync(include: "Gates");

			if (Stadiums == null)
			{
				_logger.LogError("No Stadiums found in the database.");
				return new BaseResult<IReadOnlyList<VenuesDto>>()
				{
					IsSuccess = false,
					Message = "No Stadiums Found",
					StatusCode = (int)HttpStatusCode.NotFound,
				};
			}

			var VenuesDtos = Stadiums.Select(x => new VenuesDto
			{
				Name = x.Name,
				Capacity = x.Capacity,
				City = x.City,
				Description = x.Description,
				ImageUrl = x.ImageUrl,
				Location = x.Location,
				MetroAccess = x.MetroAccess,
				GateName = x.Gates.Select(x => x.GateName).ToList(),
				AllocatedFor = x.Gates.Select(x => x.AllocatedFor).ToList(),
			}).ToList();

			return new BaseResult<IReadOnlyList<VenuesDto>>
			{
				Message = "Data Retireve Succesffuly",
				IsSuccess = true,
				StatusCode = (int)HttpStatusCode.OK,
				Data = VenuesDtos
			};
		}
	}
}
