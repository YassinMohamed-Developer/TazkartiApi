namespace Tazkarti.Domain.Entities;

public class StadiumVenue
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? MetroAccess { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }

    public ICollection<VenueGate> Gates { get; set; } = new List<VenueGate>();
    public ICollection<FootballMatch> Matches { get; set; } = new List<FootballMatch>();
    public ICollection<EntertainmentEvent> Events { get; set; } = new List<EntertainmentEvent>();
}
