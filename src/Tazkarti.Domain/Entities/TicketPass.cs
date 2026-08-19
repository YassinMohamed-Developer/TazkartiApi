using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class TicketPass
{
    public int Id { get; set; }
    public int BookingOrderId { get; set; }
    public string CurrentFanId { get; set; } = string.Empty;
    public string OriginalFanId { get; set; } = string.Empty;
    public string HolderName { get; set; } = string.Empty;
    public string? Row { get; set; }
    public string? SeatNumber { get; set; }
    public decimal Price { get; set; }
    public string? Gate { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Confirmed;
    public bool IsTransferred { get; set; } = false;

    public BookingOrder BookingOrder { get; set; } = null!;
    public ICollection<TicketTransfer> Transfers { get; set; } = new List<TicketTransfer>();
}
