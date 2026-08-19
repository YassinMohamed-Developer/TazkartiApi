using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class TicketTransfer
{
    public int Id { get; set; }
    public int TicketPassId { get; set; }
    public string SenderFanId { get; set; } = string.Empty;
    public string RecipientFanId { get; set; } = string.Empty;
    public DateTime TransferredAt { get; set; } = DateTime.UtcNow;
    public bool OtpVerified { get; set; } = false;
    public TransferStatus Status { get; set; } = TransferStatus.Pending;

    public TicketPass TicketPass { get; set; } = null!;
}
