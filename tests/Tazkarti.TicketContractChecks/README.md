# Ticket contract checks

Run `dotnet run --project tests/Tazkarti.TicketContractChecks` from the repository root.
This executable regression suite uses the existing application dependencies and requires no database.
It checks mixed-list and single-ticket JSON, ownership filtering, missing relationships,
invalid inputs, and event tickets without a venue. It also checks generated SQL using the SQL Server provider without connecting to a database. It does not measure live database performance.

## API contract

`GET /api/TicketPass/GetAllTickets` and `GET /api/TicketPass/GetTicket/{TicketPassId}`
return the existing result wrapper with a unified ticket payload:

- `id`, `type` (`Match` or `Event`), `bookingOrderId`, `currentFanId`, `holderName`,
  `price`, `gate`, `status`, `isActive`, and `details`.
- Match-only fields such as `homeTeam`, `awayTeam`, `competition`, and `round` now
  live under `details`. Match scheduling, venue, and category information is included there too.
- Event details contain `eventId`, `title`, `category`, `artist`, `eventDate`,
  `eventTime`, `city`, `venueName`, `bannerImage`, `tierId`, `tierName`, and `perks`.
- Each details object contains only fields for its ticket type. Genuinely optional
  fields within that type, such as event venue or artist, can still be null.
- `type` is serialized as a string; other enums retain their existing serialization.
- Empty lists return 200 with `data: []`. Unknown or unowned ticket IDs return 404.
  Missing required booking relationships or an event tier belonging to another event
  return 409 for single tickets and 400 for lists, instead of silently omitting a ticket.

Clients must move reads of match fields from the ticket root to `ticket.details`
and choose their detail view using `ticket.type`. The pre-existing
`GetEntertainmentEventTicket` endpoint retains its original contract for compatibility.

No schema migration is required. Match text retains existing issued snapshots with
live fallback; event details and scheduling/venue data are read from booking relationships.

## Query strategy

No Includes are used by the unified ticket endpoints. A small projection reads ticket
IDs and booking types. Detail projections select only response fields and validation
keys, joining only the selected type's tables.

A single-ticket request uses two queries. A list uses one identity query plus one
batched query per type present (at most three queries total, not one per ticket).
Empty lists and absent types do not trigger detail queries. Ownership is checked
on every query. If a ticket disappears or changes type between reads, the request
returns a controlled error rather than silently losing it. These reads do not provide
transactional snapshot consistency. Compare latency and execution plans on realistic
data before claiming a speedup: fewer selected columns trade off against extra round trips.
