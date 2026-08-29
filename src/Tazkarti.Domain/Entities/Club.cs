namespace Tazkarti.Domain.Entities;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? PrimaryColor { get; set; }

    public ICollection<AppUser> Fans { get; set; } = new List<AppUser>();
    public ICollection<FootballMatch> HomeMatches { get; set; } = new List<FootballMatch>();
    public ICollection<FootballMatch> AwayMatches { get; set; } = new List<FootballMatch>();
}
