using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Features.Ticket.Query;

public static class TicketResponseMapper
{
    public const string Includes = "BookingOrder.Match.HomeTeam,BookingOrder.Match.AwayTeam,BookingOrder.Match.Venue,BookingOrder.Category,BookingOrder.Event.Venue,BookingOrder.Tier";

    public static bool TryMap(TicketPass ticket, out TicketDto? result)
    {
        result = null;
        var booking = ticket.BookingOrder;
        if (booking == null) return false;
        TicketDetailsDto details;
        bool? isActive;
        switch (booking.BookingType)
        {
            case BookingType.Match when booking.Match is { } match:
                details = new MatchTicketDetailsDto
                {
                    MatchId = match.Id,
                    Title = ticket.Title ?? match.Title,
                    Competition = ticket.Competition ?? match.Competition,
                    Round = ticket.Round ?? match.Round,
                    HomeTeam = ticket.HomeTeam ?? match.HomeTeam?.Name ?? string.Empty,
                    AwayTeam = ticket.AwayTeam ?? match.AwayTeam?.Name ?? string.Empty,
                    MatchDate = match.MatchDate, KickoffTime = match.KickoffTime,
                    GateOpenTime = match.GateOpenTime, City = match.City,
                    VenueName = match.Venue?.Name, BannerImage = match.BannerImage,
                    CategoryId = booking.CategoryId, CategoryName = booking.Category?.Name,
                    Block = booking.Block
                };
                isActive = ticket.IsActive ?? match.IsActive;
                break;
            case BookingType.Event when booking.Event is { } ev && booking.Tier is { } tier
                && tier.EventId == ev.Id:
                details = new EventTicketDetailsDto
                {
                    EventId = ev.Id, Title = ev.Title, Category = ev.Category, Artist = ev.Artist,
                    EventDate = ev.EventDate, EventTime = ev.EventTime, City = ev.City,
                    VenueName = ev.Venue?.Name, BannerImage = ev.BannerImage,
                    TierId = tier.Id, TierName = tier.Name, Perks = tier.Perks
                };
                isActive = ticket.IsActive ?? ev.IsActive;
                break;
            default:
                return false;
        }
        result = new TicketDto
        {
            Id = ticket.Id, Type = booking.BookingType, BookingOrderId = ticket.BookingOrderId,
            CurrentFanId = ticket.CurrentFanId, HolderName = ticket.HolderName,
            Price = ticket.Price, Gate = ticket.Gate, Status = ticket.Status,
            IsActive = isActive, Details = details
        };
        return true;
    }
}
