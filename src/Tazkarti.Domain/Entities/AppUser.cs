using Microsoft.AspNetCore.Identity;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Domain.Entities;

public class AppUser : IdentityUser<string>
{
    public string FanId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public Governorate Governorate { get; set; } = Governorate.Cairo;
    public Nationality Nationality { get; set; } = Nationality.Egyptian;
    public string? AvatarUrl { get; set; }
    public LoyaltyTier LoyaltyTier { get; set; } = LoyaltyTier.Silver;
    public int AttendancePoints { get; set; } = 0;
    public bool IsVerified { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public ICollection<BookingOrder> Bookings { get; set; } = new List<BookingOrder>();
    public ICollection<AttendanceHistory> AttendanceHistories { get; set; } = new List<AttendanceHistory>();
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}
