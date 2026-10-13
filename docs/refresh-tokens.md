# Session-based refresh tokens

The implementation uses the requested `CreateAsync`, `RefreshAsync`, and `IsValidAsync` methods in
`Infrastructure/ImplmentationService/RefreshTokenService.cs`, with no extra token-generation helpers.

- **Domain**: `UserSession` and `RefreshToken` entities.
- **Application**: `IRefreshTokenService`, DTOs, and MediatR login/refresh handlers.
- **Infrastructure**: EF mappings/migration, transactional rotation, JWT generation, and session validation.
- **API**: thin controller actions dispatch MediatR commands.

The general repositories and unit of work remain unchanged. Conditional EF updates and the session
row lock keep consumption and replacement atomic, using the service flow supplied for this feature.

## Database setup

Apply the new migration before using sign-in or refresh:

```powershell
dotnet ef database update --project src/Tazkarti.Infrastructure --startup-project src/Tazkarti.Api
```

`Token:AccessTokenMinutes` defaults to 15 and `Token:RefreshTokenDays` defaults to 7.
Both options can be overridden in configuration. All timestamps use UTC and the injected `TimeProvider`.

## Client flow

1. `POST /api/Auth/SignIn` with the existing login body. After checking the password, the login handler
   calls `CreateAsync`. The response retains `data.token` and `data.tokenType` and adds `data.expiresAt`,
   `data.refreshToken`, and `data.refreshTokenExpiresAt`.
2. Use `data.token` as the bearer credential for protected endpoints.
3. `POST /api/Auth/RefreshToken` with `{ "refreshToken": "<latest refresh token>" }` to get a new pair.
   This endpoint also works after the access token expires. Replace both client credentials after success.

Each login creates an independent session. Refresh tokens are 64 random bytes encoded as 128 hex
characters; only SHA-256 hashes are stored. Rotation keeps the original session expiry rather than
extending it. Unknown, expired, invalidated, or consumed tokens return 401.

Reusing a consumed refresh token revokes its entire session, including the replacement token and
its access JWTs. Serialize client refresh requests: two competing requests count as token reuse.

JWTs include `UserId`, `session_id`, `security_stamp`, roles, and a unique `jti`. After signature and
lifetime validation, `SessionJwtBearerEvents` calls `IsValidAsync` to check ownership, revocation,
session expiry, the current Identity security stamp, and lockout. JWTs issued before this change
lack session claims and require signing in again.

Revocation is triggered by token reuse in this feature; there is no added logout endpoint.
Expired/revoked rows are retained. The migration is provided but is not applied automatically.

## Verification

```powershell
dotnet build Tazkarti.slnx
dotnet run --project tests/Tazkarti.RefreshTokenChecks
```
