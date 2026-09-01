using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.ResponseDto
{
	public record MatchDto
	{
		public string Title { get; set; } = null!;
		public string Competition { get; set; } = null!;
		public string? Round { get; set; }
		public string City { get; set; } = null!;
		public DateTime MatchDate { get; set; }
		public string KickoffTime { get; set; } = null!;
		public string? GateOpenTime { get; set; }
		public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
		public int AvailabilityPercent { get; set; } = 100;
		public decimal MinPrice { get; set; }
		public string? BannerImage { get; set; }

		public string HomeTeamName { get; set; } = null!;

		public string AwayTeamName { get; set; } = null!;
		public string VenueName { get; set; } = null!;

		public IReadOnlyList<string> NameOfCategoryMatch { get; set; } = null!;

		public IReadOnlyList<decimal> Price { get; set; } = null!;

		public IReadOnlyList<int>? Available { get; set; }

		public IReadOnlyList<string>? GateAllocation { get; set; }


	}
}
