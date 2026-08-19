namespace Tazkarti.Domain.Entities;

public class AttendanceHistory
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string MatchTitle { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public DateTime AttendanceDate { get; set; }
    public int PointsEarned { get; set; } = 0;
    public string Status { get; set; } = "Attended";

    public AppUser User { get; set; } = null!;
}
