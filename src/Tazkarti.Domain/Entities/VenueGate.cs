namespace Tazkarti.Domain.Entities;

public class VenueGate
{
    public int Id { get; set; }
    public int VenueId { get; set; }
    public string GateName { get; set; } = string.Empty;
    public string? AllocatedFor { get; set; }

    public StadiumVenue Venue { get; set; } = null!;
}
