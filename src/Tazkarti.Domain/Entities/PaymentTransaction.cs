using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class PaymentTransaction
{
    public int Id { get; set; }
    public int BookingOrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EGP";
    public string? CardNumberMasked { get; set; }
    public string? FawryCode { get; set; }
    public string? MobileWalletNumber { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Success;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public BookingOrder BookingOrder { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}
