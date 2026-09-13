using System.Text.Json.Serialization;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.RequestDto;

public record TicketDto
{
    public int Id { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter<BookingType>))]
    public BookingType Type { get; init; }
    public int BookingOrderId { get; init; }
    public string CurrentFanId { get; init; } = string.Empty;
    public string HolderName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string? Gate { get; init; }
    public TicketStatus Status { get; init; }
    public bool? IsActive { get; init; }
    public required TicketDetailsDto Details { get; init; }
}

[JsonDerivedType(typeof(MatchTicketDetailsDto))]
[JsonDerivedType(typeof(EventTicketDetailsDto))]
public abstract record TicketDetailsDto;

public record MatchTicketDetailsDto : TicketDetailsDto
{
    public int MatchId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Competition { get; init; } = string.Empty;
    public string? Round { get; init; }
    public string HomeTeam { get; init; } = string.Empty;
    public string AwayTeam { get; init; } = string.Empty;
    public DateTime MatchDate { get; init; }
    public string KickoffTime { get; init; } = string.Empty;
    public string? GateOpenTime { get; init; }
    public string City { get; init; } = string.Empty;
    public string? VenueName { get; init; }
    public string? BannerImage { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public string? Block { get; init; }
}

public record EventTicketDetailsDto : TicketDetailsDto
{
    public int EventId { get; init; }
    public string Title { get; init; } = string.Empty;
    public EventCategory Category { get; init; }
    public string? Artist { get; init; }
    public DateTime EventDate { get; init; }
    public string EventTime { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string? VenueName { get; init; }
    public string? BannerImage { get; init; }
    public int TierId { get; init; }
    public string TierName { get; init; } = string.Empty;
    public string? Perks { get; init; }
}
