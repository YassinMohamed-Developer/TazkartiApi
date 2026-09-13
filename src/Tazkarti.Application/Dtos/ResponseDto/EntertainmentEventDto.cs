using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.ResponseDto
{
	public record EntertainmentEventDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public EventCategory Category { get; set; }
		public string? Tag { get; set; }
		public string? Artist { get; set; }
		public DateTime EventDate { get; set; }
		public string EventTime { get; set; } = string.Empty;
		public int? VenueId { get; set; }
		public string? VenueName { get; set; }
		public string City { get; set; } = string.Empty;
		public decimal MinPrice { get; set; }
		public string? BannerImage { get; set; }
		public string? Description { get; set; }
		public bool IsActive { get; set; }

		public IReadOnlyList<int> TierId { get; set; }
		public IReadOnlyList<string> NameOfTicketTier { get; set; } = new List<string>();
		public IReadOnlyList<decimal> Price { get; set; } = new List<decimal>();
		public IReadOnlyList<string?> Perks { get; set; } = new List<string?>();
	}
}
