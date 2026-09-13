using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public record EntertainmentEventTicketDto
	{
		public int TicketPassId { get; set; }
		public int BookingOrderId { get; set; }
		public string CurrentFanId { get; set; } = string.Empty;
		public string HolderName { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string? Gate { get; set; }
		public TicketStatus Status { get; set; }
		public int EventId { get; set; }
		public string Title { get; set; } = string.Empty;
		public EventCategory Category { get; set; }
		public string? Artist { get; set; }
		public DateTime EventDate { get; set; }
		public string EventTime { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string? VenueName { get; set; }
		public string? BannerImage { get; set; }
		public bool IsActive { get; set; }
		public int TierId { get; set; }
		public string TierName { get; set; } = string.Empty;
		public string? Perks { get; set; }
	}
}
