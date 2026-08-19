namespace Tazkarti.Domain.Entities;

public class EventTicketTier
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Perks { get; set; } // Stored as JSON string

    public EntertainmentEvent Event { get; set; } = null!;
}
