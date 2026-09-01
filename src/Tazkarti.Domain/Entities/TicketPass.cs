using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class TicketPass
{
    public int Id { get; set; }
    public int BookingOrderId { get; set; }
    public string CurrentFanId { get; set; } = string.Empty;
    public string OriginalFanId { get; set; } = string.Empty;
    public string HolderName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Gate { get; set; }
    public TicketStatus Status { get; set; }
    public bool IsTransferred { get; set; } = false;
    public string? Competition { get; set; }
    public string? Round { get; set; }
    public string? Title { get; set; }
    public string? HomeTeam { get; set; }
    public string? AwayTeam { get; set; }

    public bool? IsActive { get; set; } = true;
	public BookingOrder BookingOrder { get; set; } = null!;
    public ICollection<TicketTransfer> Transfers { get; set; } = new List<TicketTransfer>();
}
