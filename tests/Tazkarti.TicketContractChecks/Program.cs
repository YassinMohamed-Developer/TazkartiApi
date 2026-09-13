using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Features.Ticket.Query;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;
using Tazkarti.Domain.Interfaces;

var checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
}
var match = new TicketPass
{
    Id = 1, BookingOrderId = 11, HomeTeam = "Issued home", Price = 300,
    BookingOrder = new BookingOrder
    {
        UserId = "owner", BookingType = BookingType.Match,
        Match = new FootballMatch { Id = 21, Title = "Match", HomeTeam = new Club { Name = "Live home" }, AwayTeam = new Club { Name = "Away" } }
    }
};
var ev = new TicketPass
{
    Id = 2, BookingOrderId = 12, IsActive = null, Price = 500,
    BookingOrder = new BookingOrder
    {
        UserId = "owner", BookingType = BookingType.Event,
        Event = new EntertainmentEvent { Id = 22, Title = "Concert", Artist = "Artist", IsActive = true },
        Tier = new EventTicketTier { Id = 32, EventId = 22, Name = "VIP" }
    }
};
var repo = DispatchProxy.Create<IGenericRepository<TicketPass>, RepositoryStub>();
var stub = (RepositoryStub)(object)repo;
stub.Tickets.AddRange([match, ev]);
var work = DispatchProxy.Create<IUnitOfWork, WorkStub>();
((WorkStub)(object)work).Repository = repo;
var all = new GetAllTicketsHandler(work);
var single = new GetTicketHandler(work);
var list = await all.Handle(new("owner"), default);
Check(list.IsSuccess && list.Data.Count == 2, "Mixed list must contain both ticket types.");
Check(stub.Calls == 3, "Mixed list must use one identity read and two batched detail reads.");
Check(list.Data[0].Details is MatchTicketDetailsDto { HomeTeam: "Issued home", AwayTeam: "Away" }, "Match text must preserve snapshots and support live fallback.");
Check(list.Data[1].Details is EventTicketDetailsDto { TierName: "VIP", VenueName: null }, "Optional event venue must not reject the ticket.");
Check(list.Data[1].IsActive == true, "Event ticket with no stored activity must use event activity.");
var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
using var json = JsonDocument.Parse(JsonSerializer.Serialize(list, options));
var items = json.RootElement.GetProperty("data");
Check(items[0].GetProperty("type").GetString() == "Match" && items[1].GetProperty("type").GetString() == "Event", "Type must be a stable string discriminator.");
Check(items[0].GetProperty("details").GetProperty("homeTeam").GetString() == "Issued home", "Derived match fields must serialize inside lists.");
Check(items[1].GetProperty("details").GetProperty("artist").GetString() == "Artist", "Derived event fields must serialize inside lists.");
Check(!items[0].GetProperty("details").TryGetProperty("artist", out _) && !items[1].GetProperty("details").TryGetProperty("homeTeam", out _), "Unrelated type fields must be absent.");
Check(!items[0].TryGetProperty("homeTeam", out _), "Type-specific fields belong only under details.");
var beforeSingle = stub.Calls;
var detail = await single.Handle(new("owner", 2), default);
Check(stub.Calls - beforeSingle == 2, "Single lookup must use only identity and selected-type reads.");
Check(detail.IsSuccess && JsonSerializer.Serialize(detail.Data, options) == JsonSerializer.Serialize(list.Data[1], options), "Single and list endpoints must share the same contract.");
Check((await single.Handle(new("other", 2), default)).StatusCode == 404, "Another user must not read the ticket.");
Check((await single.Handle(new("owner", 999), default)).StatusCode == 404, "Unknown ticket must return 404.");
Check((await all.Handle(new("other"), default)).Data.Count == 0, "No tickets must return an empty list.");
stub.Tickets.Remove(ev);
var beforeMatchOnly = stub.Calls;
Check((await all.Handle(new("owner"), default)).Data.Count == 1 && stub.Calls - beforeMatchOnly == 2, "Match-only list must skip the event detail query.");
stub.Tickets.Add(ev);
var calls = stub.Calls;
Check((await all.Handle(new(null), default)).StatusCode == 401, "Missing identity must be rejected.");
Check((await single.Handle(new("owner", 0), default)).StatusCode == 400, "Invalid ticket ID must be rejected.");
Check(stub.Calls == calls, "Invalid inputs must not query the repository.");
ev.BookingOrder.Tier = null;
Check((await single.Handle(new("owner", 2), default)).StatusCode == 409, "Missing event tier must return controlled conflict.");
Check((await all.Handle(new("owner"), default)).StatusCode == 400, "Mixed list must not silently omit incomplete tickets.");
ev.BookingOrder.Tier = new EventTicketTier { EventId = 999 };
Check(TicketResponseMapper.Event.Compile()(ev).Details == null, "A tier belonging to another event must be rejected.");
ev.BookingOrder.Event = null;
Check(TicketResponseMapper.Event.Compile()(ev).Details == null, "Missing event must not throw.");
match.BookingOrder.Match = null;
Check(TicketResponseMapper.Match.Compile()(match).Details == null, "Missing match must not throw.");
match.BookingOrder.BookingType = (BookingType)99;
Check((await single.Handle(new("owner", 1), default)).StatusCode == 409, "Unknown booking type must not become an event.");
var sqlOptions = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<Tazkarti.Infrastructure.Data.TazkartiDbContext>()
    .UseSqlServer("Server=localhost;Database=TicketProjectionChecks;Trusted_Connection=True;TrustServerCertificate=True").Options;
using var context = new Tazkarti.Infrastructure.Data.TazkartiDbContext(sqlOptions);
var query = context.TicketPasses.Where(t => t.BookingOrder.UserId == "owner" && t.Id == 1);
var identitySql = query.Select(TicketResponseMapper.Identity).Take(1).ToQueryString();
var matchSql = query.Where(t => t.BookingOrder.BookingType == BookingType.Match).Select(TicketResponseMapper.Match).Take(1).ToQueryString();
var eventSql = query.Where(t => t.BookingOrder.BookingType == BookingType.Event).Select(TicketResponseMapper.Event).Take(1).ToQueryString();
Check(!identitySql.Contains("[FootballMatches]") && !identitySql.Contains("[EntertainmentEvents]"), "Identity read must not join detail tables.");
Check(!matchSql.Contains("[EntertainmentEvents]") && !matchSql.Contains("[EventTicketTiers]"), "Match SQL must not join event tables.");
Check(!eventSql.Contains("[FootballMatches]") && !eventSql.Contains("[Clubs]") && !eventSql.Contains("[MatchTicketCategories]"), "Event SQL must not join match tables.");
foreach (var sql in new[] { matchSql, eventSql })
    Check(!sql.Contains("[PaymentMethod]") && !sql.Contains("[Description]") && !sql.Contains("[Capacity]") && !sql.Contains("[OriginalFanId]"), "Projection must omit unused entity columns.");
// Also translate the batched IN queries, without opening a database connection.
var ids = new[] { 1, 2 };
Check(context.TicketPasses.Where(t => ids.Contains(t.Id)).Select(TicketResponseMapper.Match).ToQueryString().Contains("WHERE"), "Batch projection must translate to SQL.");
Console.WriteLine($"Passed {checks} ticket contract and SQL checks.");

public class WorkStub : DispatchProxy
{
    public required IGenericRepository<TicketPass> Repository { get; set; }
    protected override object? Invoke(MethodInfo? method, object?[]? args) => method?.Name == "Repository"
        ? Repository : throw new NotSupportedException(method?.Name);
}
public class RepositoryStub : DispatchProxy
{
    public List<TicketPass> Tickets { get; } = [];
    public int Calls { get; private set; }
    protected override object? Invoke(MethodInfo? method, object?[]? args)
    {
        Calls++;
        return typeof(RepositoryStub).GetMethod(nameof(Project), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(method!.GetGenericArguments()[0]).Invoke(this, [method.Name, args]);
    }
    private object Project<TResult>(string method, object?[] args)
    {
        var predicate = ((Expression<Func<TicketPass, bool>>)args[0]!).Compile();
        var selector = ((Expression<Func<TicketPass, TResult>>)args[1]!).Compile();
        var rows = Tickets.Where(predicate).Select(selector).ToList();
        return method switch
        {
            "FindAllAndProjectAsync" => Task.FromResult<IReadOnlyList<TResult>>(rows),
            "FindAndProjectAsync" => Task.FromResult(rows.FirstOrDefault()),
            _ => throw new NotSupportedException(method)
        };
    }
}

