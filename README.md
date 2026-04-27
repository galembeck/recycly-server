<div align="center">

<img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/EF%20Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
<img src="https://img.shields.io/badge/SignalR-Real--time-00AE6C?style=for-the-badge" />
<img src="https://img.shields.io/badge/Hangfire-Jobs-orange?style=for-the-badge" />
<img src="https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" />

<br /><br />

# ♻️ Recycly — Backend API

**RESTful API for a recycling ecosystem connecting citizens, cooperatives, and materials.**  
Built with ASP.NET Core 10, Clean Architecture, and Entity Framework Core.

[Overview](#-overview) · [Architecture](#-architecture) · [Tech Stack](#-tech-stack) · [Folder Structure](#-folder-structure) · [API Reference](#-api-reference) · [Getting Started](#-getting-started)

</div>

---

## 📌 Overview

Recycly is a platform that bridges the gap between people who want to recycle and the cooperatives that collect and process recyclable materials. This repository contains the **backend API**, responsible for:

- **Authentication & Authorization** — JWT-based authentication with cookie transport and refresh token rotation, supporting three distinct user roles: `CLIENT`, `COOPERATIVE`, and `ADMIN`.
- **Collection Point Management** — Cooperatives register physical drop-off points with automatic geocoding (ZIP-first strategy via Nominatim/OpenStreetMap).
- **Material Tracking** — Custom recyclable material types with names and color coding, linked to collection points and collects.
- **Collect History** — Full audit trail of recycling collects, linked to cooperatives, collection points, and materials.
- **Sales Management** — Cooperatives register sales of recyclable materials, tracking buyers, weight, revenue, and associated materials (many-to-many).
- **Dashboard Analytics** — Aggregated stats: total revenue, collect counts, kg by material category, and a 90-day daily chart (collects × sales).
- **Statistics Engine** — Monthly kg evolution (6-month window), material distribution, weekly activity patterns, and unlockable cooperative achievements.
- **Real-time Notifications** — SignalR hub for admin-facing live events.
- **Background Jobs** — Hangfire for async processing (email, tracking codes).
- **File Storage** — Document/avatar upload support per user.

---

## 🏛 Architecture

The solution follows **Clean Architecture** with four independent projects and a strict unidirectional dependency flow:

```
┌─────────────────────────────────────────────────────────┐
│                      API.Public                         │
│   Controllers · DTOs · Validators · Filters · Hubs      │
│   Middlewares · Extensions · Configuration              │
└────────────────────────┬────────────────────────────────┘
                         │ depends on
┌────────────────────────▼────────────────────────────────┐
│                        Domain                           │
│   Entities · Service Interfaces & Implementations       │
│   Repository Interfaces · Enumerators · Exceptions      │
│   Utils · SearchParameters · Constants                  │
└───────────┬─────────────────────────────┬───────────────┘
            │ ← depends on               │ ← depends on
┌───────────▼───────────┐   ┌────────────▼───────────────┐
│      Repository       │   │           IoC              │
│  AppDbContext · EF    │   │  NativeInjector.cs         │
│  Configurations       │   │  Wires all DI bindings     │
│  Repository Impls     │   │  Cache · Logger setup      │
└───────────────────────┘   └────────────────────────────┘
```

**Dependency rule:** `API.Public` → `Domain` ← `Repository` ← `IoC`  
`IoC` references all layers solely to register bindings; it is never referenced by others.

### Key Design Patterns

| Pattern | Implementation |
|---|---|
| **Repository** | `BaseRepository<T>` provides generic CRUD; each entity has a typed `IRepository` interface in `Domain` and a concrete implementation in `Repository` |
| **Service Layer** | Business logic lives exclusively in `Domain/Services`; controllers call services, never repositories directly |
| **Generic Base Service** | `IService<E, R, S>` abstract class wires common CRUD operations to any entity/repository pair |
| **Soft Delete** | All entities track `DeletedAt`, `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` via `BaseEntity` |
| **Validation** | FluentValidation validators in `API.Public/Validators` are auto-registered; error codes are enum-driven strings |
| **Auth Cookies** | JWT tokens transported as `HttpOnly + Secure` cookies with refresh token rotation |
| **Role-based Access** | Custom `[Filters.Authorize(ProfileType...)]` attribute on top of JWT bearer |

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10.0 |
| Web Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server (LocalDB for dev) |
| Authentication | JWT Bearer + Refresh Tokens (cookie transport) |
| Validation | FluentValidation 12 |
| Background Jobs | Hangfire 1.8 (SQL Server storage) |
| Real-time | SignalR |
| Geocoding | Nominatim / OpenStreetMap (via `HttpClient`) |
| API Docs | Scalar (OpenAPI) |
| Logging | Serilog (Console + MSSQL sinks) |
| Rate Limiting | AspNetCoreRateLimit |
| Security Headers | OwaspHeaders.Core |
| Email | Resend SDK *(configured, inactive)* |
| Password Hashing | BCrypt.Net-Next |
| User-Agent Parsing | UAParser |

---

## 📁 Folder Structure

```
recycly-server/
│
├── API.Public/                        # Entry point — ASP.NET Core Web API
│   ├── Configuration/                 # Bootstrapping helpers (JWT, DB, Hangfire,
│   │                                  #   compression, rate limit, controllers)
│   ├── Controllers/
│   │   ├── _Base/                     # _BaseController: cookie helpers, security info,
│   │   │                              #   IdentityPrincipal accessor
│   │   ├── AuthController.cs          # Sign-in, sign-up, refresh, sign-out
│   │   ├── UserController.cs          # Profile management, document upload
│   │   ├── MaterialController.cs      # Material CRUD (COOPERATIVE/ADMIN)
│   │   ├── CollectionPointController.cs # Point CRUD + materials update + geocoding
│   │   ├── CollectController.cs       # Collect history (user, cooperative, by point)
│   │   ├── SaleController.cs          # Sale registration & management
│   │   ├── DashboardController.cs     # Aggregated dashboard stats
│   │   └── StatisticsController.cs    # Monthly/weekly/material/achievement stats
│   ├── DTOs/                          # Inbound and outbound data shapes
│   │   ├── Auth/
│   │   ├── User/
│   │   ├── Material/
│   │   ├── CollectionPoint/
│   │   ├── Collect/
│   │   └── Sale/
│   ├── Extensions/
│   │   └── ServiceCollectionExtension.cs  # Single entry point for all service wiring
│   ├── Filters/
│   │   └── AuthorizeAttribute.cs      # Role-based authorization filter
│   ├── Hubs/
│   │   └── AdminNotificationHub.cs    # SignalR hub for real-time admin events
│   ├── Middlewares/
│   │   ├── ExceptionMiddleware.cs     # Global exception → structured error response
│   │   ├── CorrelationIdMiddleware.cs # X-Correlation-ID propagation
│   │   └── SecureHeadersMiddleware.cs # OWASP security headers
│   ├── Validators/                    # FluentValidation — auto-registered
│   │   ├── Material/
│   │   ├── CollectionPoint/
│   │   ├── Collect/
│   │   └── Sale/
│   ├── appsettings.json               # Base configuration (no secrets)
│   ├── appsettings.Development.json   # ⚠️  Git-ignored — add your secrets here
│   └── Program.cs                     # Minimal hosting setup
│
├── Domain/                            # Pure business logic — no framework dependencies
│   ├── Data/
│   │   ├── Entities/                  # EF entity classes (mapped to TBxxx tables)
│   │   │   ├── _Base/                 # BaseEntity (id, audit fields, soft delete)
│   │   │   ├── User/
│   │   │   ├── Material/
│   │   │   ├── CollectionPoint/
│   │   │   ├── Collect/
│   │   │   └── Sale/
│   │   └── Models/                    # Internal DTOs, result models
│   ├── Enumerators/
│   │   ├── ProfileType.cs             # ADMIN=1, CLIENT=2, COOPERATIVE=3
│   │   ├── BusinessErrorMessage.cs    # Error code enum (string descriptions)
│   │   └── ValidationErrorMessage.cs  # Validation error code enum
│   ├── Exceptions/                    # DomainException, PersistenceException, etc.
│   ├── Repository/                    # Repository interfaces (contracts only)
│   │   ├── _Base/IRepository<T>
│   │   ├── User/
│   │   ├── Material/
│   │   ├── CollectionPoint/
│   │   ├── Collect/
│   │   └── Sale/
│   ├── Services/                      # Business logic — interfaces + implementations
│   │   ├── _Base/IService<E,R,S>      # Generic base service with common CRUD
│   │   ├── Auth/
│   │   ├── User/
│   │   ├── Material/
│   │   ├── CollectionPoint/           # Includes ResolveCoordinatesAsync (ZIP-first)
│   │   ├── Collect/
│   │   ├── Sale/
│   │   ├── Dashboard/                 # DashboardStats — 90-day chart + kg by material
│   │   ├── Statistics/                # Monthly kg, weekly activity, achievements
│   │   ├── Geocoding/                 # Nominatim wrapper with ZIP-first fallback
│   │   ├── FileStorage/
│   │   └── Email/
│   ├── Constants/                     # App-wide constants, Settings model
│   └── Utils/                         # SecurityUtil, RegexUtil, EnumHelper, etc.
│
├── Repository/                        # Data access — EF Core implementations
│   ├── AppDbContext.cs                # DbContext with all DbSets
│   ├── Configuration/                 # Fluent API entity configs
│   │   ├── Material/
│   │   ├── CollectionPoint/
│   │   ├── Collect/
│   │   └── Sale/                      # M2M Sale↔Material via TBSaleMaterial
│   ├── Migrations/                    # EF Core generated migrations
│   └── Repository/                    # Concrete repository implementations
│       ├── _Base/BaseRepository<T>    # Generic CRUD + soft delete
│       ├── User/
│       ├── Material/
│       ├── CollectionPoint/
│       ├── Collect/
│       └── Sale/
│
└── IoC/                               # Dependency injection wiring
    ├── NativeInjector.cs              # All AddScoped<Interface, Implementation> calls
    ├── CacheConfiguration.cs
    ├── LoggerConfiguration.cs
    └── CacheInitializer.cs
```

---

## 🔌 API Reference

All endpoints are prefixed by the controller name (e.g. `/auth`, `/collectionpoint`).  
Interactive docs available at `http://localhost:5005/scalar/v1` when running locally.

### Auth

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `POST` | `/auth/sign-up` | Public | Register a new user (CLIENT or COOPERATIVE) |
| `POST` | `/auth/sign-in` | Public | Authenticate by CPF/email + password |
| `POST` | `/auth/refresh` | Cookie | Rotate access + refresh tokens |
| `POST` | `/auth/sign-out` | Cookie | Invalidate session and clear cookies |

### User

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/user/me` | Any role | Fetch authenticated user profile |
| `PUT` | `/user` | Any role | Update profile |
| `POST` | `/user/document` | Any role | Upload document/avatar |

### Material

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/material` | Public | List all materials |
| `GET` | `/material/{id}` | Public | Get material by id |
| `POST` | `/material` | COOPERATIVE / ADMIN | Create material |
| `PUT` | `/material/{id}` | COOPERATIVE / ADMIN | Update material |
| `DELETE` | `/material/{id}` | COOPERATIVE / ADMIN | Delete material |

### Collection Points

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/collectionpoint` | Public | List all points (with materials) |
| `GET` | `/collectionpoint/{id}` | Public | Get point by id |
| `GET` | `/collectionpoint/mine` | COOPERATIVE / ADMIN | Points owned by the caller |
| `POST` | `/collectionpoint` | COOPERATIVE / ADMIN | Create point — geocoding runs automatically |
| `PUT` | `/collectionpoint/{id}` | COOPERATIVE / ADMIN | Update point |
| `PUT` | `/collectionpoint/{id}/materials` | COOPERATIVE / ADMIN | Replace accepted materials |
| `DELETE` | `/collectionpoint/{id}` | COOPERATIVE / ADMIN | Soft-delete point |

### Collects

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/collect` | Any role | Collect history of the authenticated user |
| `GET` | `/collect/cooperative` | COOPERATIVE / ADMIN | All collects across the caller's cooperatives |
| `GET` | `/collect/point/{pointId}` | COOPERATIVE / ADMIN | Collects for a specific point |
| `POST` | `/collect` | COOPERATIVE / ADMIN | Register a new collect |
| `DELETE` | `/collect/{id}` | COOPERATIVE / ADMIN | Soft-delete a collect |

### Sales

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/sale` | COOPERATIVE / ADMIN | Sales of the caller's cooperatives |
| `POST` | `/sale` | COOPERATIVE / ADMIN | Register a sale (with material list) |
| `DELETE` | `/sale/{id}` | COOPERATIVE / ADMIN | Soft-delete a sale |

### Analytics

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `GET` | `/dashboard/stats` | COOPERATIVE / ADMIN | Profit, collect count, kg by material, 90-day chart |
| `GET` | `/statistics/stats` | COOPERATIVE / ADMIN | Monthly kg (6 mo), material distribution, weekly activity, achievements |

---

## 🚀 Getting Started

### Prerequisites

| Tool | Version |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ |
| [SQL Server](https://www.microsoft.com/sql-server) | 2019+ or LocalDB |
| [EF Core CLI](https://learn.microsoft.com/ef/core/cli/dotnet) | `dotnet tool install -g dotnet-ef` |

### 1. Clone the repository

```bash
git clone https://github.com/galembeck/recycly-server.git
cd recycly-server
```

### 2. Configure the environment

Create `API.Public/appsettings.Development.json` (this file is git-ignored):

```json
{
  "Settings": {
    "Environment": "Development",
    "Domain": "localhost",
    "SystemId": "system",
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RECYCLY_DEV;Trusted_Connection=True;"
    },
    "JwtSettings": {
      "SecretKey": "your-256-bit-secret-key-here",
      "Issuer": "recycly-api",
      "Audience": "recycly-client",
      "AccessTokenExpirationMinutes": 60,
      "RefreshTokenExpirationDays": 7
    },
    "AuthSettings": {
      "MaxFailedAccessAttempts": 5
    }
  }
}
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Apply database migrations

```bash
dotnet ef database update --project Repository --startup-project API.Public
```

### 5. Run the API

```bash
dotnet run --project API.Public
```

The server starts at **`http://localhost:5005`**.  
Interactive API docs: **`http://localhost:5005/scalar/v1`**  
Hangfire dashboard: **`http://localhost:5005/hangfire`**

---

## 🗄 Database

All tables are prefixed with `TB`:

```
TBUser               — accounts (CLIENT, COOPERATIVE, ADMIN)
TBUserSecurityInfo   — login fingerprint (IP, browser, MAC)
TBUserHistoric       — audit log of user actions
TBAccessToken        — active JWT access tokens
TBRefreshToken       — refresh token rotation store
TBMaterial           — recyclable material types (name + color)
TBCollectionPoint    — physical drop-off locations with geocoords
TBCollect            — individual recycling collection records
TBSale               — sales of recyclable materials
TBSaleMaterial       — join table (Sale ↔ Material, many-to-many)
```

All entities follow a **soft-delete** pattern — records are never physically removed; `DeletedAt` is set instead.

### Managing migrations

```bash
# Create a new migration
dotnet ef migrations add <MigrationName> \
  --project Repository \
  --startup-project API.Public

# Apply pending migrations
dotnet ef database update \
  --project Repository \
  --startup-project API.Public

# Roll back to a specific migration
dotnet ef database update <PreviousMigrationName> \
  --project Repository \
  --startup-project API.Public
```

---

## 🔐 Authentication Flow

```
Client                          API
  │                              │
  ├─── POST /auth/sign-in ──────►│
  │    { cpf/email, password }   │
  │                              │── verify credentials
  │                              │── generate AccessToken (JWT, 60 min)
  │                              │── generate RefreshToken (opaque, 7 days)
  │◄── Set-Cookie: AccessToken ──┤
  │◄── Set-Cookie: RefreshToken ─┤
  │                              │
  ├─── GET /any/protected ──────►│  (cookie sent automatically)
  │                              │── validate JWT → IdentityPrincipal
  │◄── 200 OK ───────────────────┤
  │                              │
  ├─── POST /auth/refresh ──────►│  (when AccessToken expires)
  │                              │── validate RefreshToken
  │                              │── rotate both tokens
  │◄── Set-Cookie (new tokens) ──┤
```

Tokens are transported exclusively as **HttpOnly, Secure cookies** — they are never exposed in the response body, protecting against XSS.

---

## 🗺 Geocoding

When a cooperative registers or updates a collection point, the API automatically resolves coordinates:

1. **ZIP-first** — queries Nominatim with the ZIP code + "Brasil"
2. **Full address fallback** — if ZIP lookup fails, retries with street, number, city, state
3. Coordinates (`Latitude`, `Longitude`) are stored as strings on the entity
4. All Nominatim requests include a `User-Agent` header and a 10-second timeout

---

## 📊 Analytics

### Dashboard Stats (`GET /dashboard/stats`)

Computed for the authenticated cooperative:

- `totalSalesRevenue` / `totalSalesProfit` — sum of all sale prices
- `totalCollectsCount` — total number of collects
- `metalKg` / `plasticKg` / `glassKg` — total weight grouped by material name (case-insensitive keyword matching)
- `chartData` — 90-day array of `{ date, collects, sales }` daily counts

### Statistics (`GET /statistics/stats`)

- `monthlyKg` — kg collected per month, last 6 months (pt-BR month abbreviations)
- `materialDistribution` — kg grouped by exact material name, descending
- `weeklyActivity` — collect count per day of week (Dom–Sáb)
- `achievements` — 5 unlockable milestones based on collect count and total kg

---

## 🧱 Adding a New Feature

Follow this checklist to add a new domain entity end-to-end:

1. **Entity** — create `Domain/Data/Entities/<Feature>/<Feature>.cs` extending `BaseEntity`
2. **Repository interface** — `Domain/Repository/<Feature>/I<Feature>Repository.cs`
3. **Service interface** — `Domain/Services/<Feature>/I<Feature>Service.cs` (abstract class extending `IService<E,R,S>`)
4. **Service implementation** — `Domain/Services/<Feature>/<Feature>Service.cs`
5. **EF configuration** — `Repository/Configuration/<Feature>/<Feature>Configuration.cs`
6. **Repository implementation** — `Repository/Repository/<Feature>/<Feature>Repository.cs`
7. **DbContext** — add `DbSet<Feature>` to `AppDbContext`
8. **DTOs** — `API.Public/DTOs/<Feature>/Create<Feature>DTO.cs`, `<Feature>ResponseDTO.cs`
9. **Validator** — `API.Public/Validators/<Feature>/Create<Feature>Validator.cs`
10. **Controller** — `API.Public/Controllers/<Feature>Controller.cs`
11. **IoC registration** — add both repository and service to `IoC/NativeInjector.cs`
12. **Migration** — `dotnet ef migrations add Add<Feature> --project Repository --startup-project API.Public`

---

## 👤 Author

**Pedro Galembeck**  
[github.com/galembeck](https://github.com/galembeck)

---

<div align="center">

Made with 💚 for a more sustainable world.

</div>
