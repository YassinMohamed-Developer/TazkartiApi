using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class EntertainmentEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public EventCategory Category { get; set; } = EventCategory.MusicAndConcerts;
    public string? Tag { get; set; }
    public string? Artist { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTime { get; set; } = string.Empty;
    public int VenueId { get; set; }
    public string City { get; set; } = string.Empty;
    public decimal MinPrice { get; set; }
    public string? BannerImage { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public StadiumVenue Venue { get; set; } = null!;
    public ICollection<EventTicketTier> TicketTiers { get; set; } = new List<EventTicketTier>();
    public ICollection<BookingOrder> Bookings { get; set; } = new List<BookingOrder>();
}
