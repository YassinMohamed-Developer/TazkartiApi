using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class FootballMatch
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Competition { get; set; } = string.Empty;
    public string? Round { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int VenueId { get; set; }
    public string City { get; set; } = string.Empty;
    public DateTime MatchDate { get; set; }
    public string KickoffTime { get; set; } = string.Empty;
    public string? GateOpenTime { get; set; }
    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
    public int AvailabilityPercent { get; set; } = 100;
    public decimal MinPrice { get; set; }
    public string? BannerImage { get; set; }
    public bool? IsActive { get; set; } = true;

    public Club HomeTeam { get; set; } = null!;
    public Club AwayTeam { get; set; } = null!;
    public StadiumVenue Venue { get; set; } = null!;
    public ICollection<MatchTicketCategory> TicketCategories { get; set; } = new List<MatchTicketCategory>();
    public ICollection<BookingOrder> Bookings { get; set; } = new List<BookingOrder>();
}
