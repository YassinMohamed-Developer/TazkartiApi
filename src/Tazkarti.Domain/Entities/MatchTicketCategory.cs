namespace Tazkarti.Domain.Entities;

public class MatchTicketCategory
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Color { get; set; }
    public int Available { get; set; }
    public string? GateAllocation { get; set; }

    public FootballMatch Match { get; set; } = null!;
}
