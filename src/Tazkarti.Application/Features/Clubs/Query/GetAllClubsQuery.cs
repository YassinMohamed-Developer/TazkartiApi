using MediatR;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Features.Clubs.Query
{
	public record GetAllClubsQueryRequest : IRequest<BaseResult<List<ClubDto>>>;
	public class GetAllClubsQuery : IRequestHandler<GetAllClubsQueryRequest, BaseResult<List<ClubDto>>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllClubsQuery(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<BaseResult<List<ClubDto>>> Handle(GetAllClubsQueryRequest request, CancellationToken cancellationToken)
		{
			var clubs = await _unitOfWork.Repository<Club>().GetAllAsync();

			if (clubs == null)
			{
				return new BaseResult<List<ClubDto>>
				{
					IsSuccess = false,
					Message = "No clubs found",
					Errors = new List<string> { "No clubs found" },
					StatusCode = 404
				};

			}
			else
			{
				var clubDtos = clubs.Select(c => new ClubDto
				{
					Id = c.Id,
					Name = c.Name
				}).ToList();
				return new BaseResult<List<ClubDto>>
				{
					IsSuccess = true,
					Message = "Clubs retrieved successfully",
					Data = clubDtos,
					StatusCode = 200
				};
			}
		}
	}
}
