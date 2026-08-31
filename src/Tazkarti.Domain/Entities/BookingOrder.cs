using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class BookingOrder
{
    public int Id { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public BookingType BookingType { get; set; }
    public int? MatchId { get; set; }
    public int? EventId { get; set; }
    public int? CategoryId { get; set; }
    public int? TierId { get; set; }
    public int VenueId { get; set; }
    public string City { get; set; } = string.Empty;
    public string? Gate { get; set; }
    public string? Block { get; set; }
    public decimal TotalAmount { get; set; }
    public int Quantity { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AppUser User { get; set; } = null!;
    public FootballMatch? Match { get; set; }
    public EntertainmentEvent? Event { get; set; }
    public MatchTicketCategory? Category { get; set; }
    public EventTicketTier? Tier { get; set; }
    public ICollection<TicketPass> Tickets { get; set; } = new List<TicketPass>();
    public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
}
