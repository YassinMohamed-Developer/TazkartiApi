namespace Tazkarti.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SessionId { get; set; }

    public string TokenHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? UsedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public Guid? ReplacedByTokenId { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Session Session { get; set; } = null!;
    public RefreshToken? ReplacedByToken { get; set; }
}
