using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.Data;

public class TazkartiDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public TazkartiDbContext(DbContextOptions<TazkartiDbContext> options) : base(options)
    {
    }

    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<StadiumVenue> StadiumVenues => Set<StadiumVenue>();
    public DbSet<VenueGate> VenueGates => Set<VenueGate>();
    public DbSet<FootballMatch> FootballMatches => Set<FootballMatch>();
    public DbSet<MatchTicketCategory> MatchTicketCategories => Set<MatchTicketCategory>();
    public DbSet<EntertainmentEvent> EntertainmentEvents => Set<EntertainmentEvent>();
    public DbSet<EventTicketTier> EventTicketTiers => Set<EventTicketTier>();
    public DbSet<BookingOrder> BookingOrders => Set<BookingOrder>();
    public DbSet<TicketPass> TicketPasses => Set<TicketPass>();
    public DbSet<TicketTransfer> TicketTransfers => Set<TicketTransfer>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<AttendanceHistory> AttendanceHistories => Set<AttendanceHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(TazkartiDbContext).Assembly);
    }
}
