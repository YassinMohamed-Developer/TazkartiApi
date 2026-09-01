using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public record TicketDto
	{
		public int BookingOrderId { get; set; }
		public string CurrentFanId { get; set; } = string.Empty;
		public string HolderName { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string? Gate { get; set; }
		public TicketStatus Status { get; set; } = TicketStatus.Confirmed;

		public string? Competition { get; set; }
		public string? Round { get; set; }
		public string? Title { get; set; }
		public string? HomeTeam { get; set; }
		public string? AwayTeam { get; set; }

		public bool? IsActive { get; set; }
	}
}
