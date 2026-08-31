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
		public string? Row { get; set; }
		public string? SeatNumber { get; set; }
		public decimal Price { get; set; }
		public string? Gate { get; set; }
		public TicketStatus Status { get; set; } = TicketStatus.Confirmed;
	}
}
