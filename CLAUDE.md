# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run (development)
dotnet run --project API.Public

# Run EF migrations
dotnet ef migrations add <Name> --project Repository --startup-project API.Public
dotnet ef database update --project Repository --startup-project API.Public

# Restore packages
dotnet restore
```

The dev server runs at `http://localhost:5005`. API docs are at `http://localhost:5005/scalar/v1`.

## Architecture

**Clean Architecture** across 4 projects in `recycly-server.slnx`:

- **`API.Public`** — ASP.NET Core 10 Web API. Entry point (`Program.cs`), controllers, DTOs, validators, middleware, SignalR hubs, Hangfire dashboard.
- **`Domain`** — Entities (`Data/Entities/`), service interfaces and implementations (`Services/`), enumerators, exceptions, and internal models (`Data/Models/`).
- **`Repository`** — `AppDbContext`, EF Core fluent configurations (`Configuration/`), migrations, and repository implementations (`Repository/`).
- **`IoC`** — Single `NativeInjector.cs` that wires all service-to-interface bindings; cache and logger configuration.

**Dependency flow:** `API.Public` → `Domain` ← `Repository` ← `IoC` (IoC references all layers to register bindings).

## Key Patterns

**Repository pattern:** `BaseRepository<T>` in `Repository/Repository/_Base/` provides generic CRUD. Each entity has a typed interface in `Domain/Repository/` and implementation in `Repository/Repository/<Entity>/`.

**Service layer:** Business logic lives in `Domain/Services/<Feature>/`. Controllers in `API.Public/Controllers/` call services, never repositories directly.

**Validation:** FluentValidation validators in `API.Public/Validators/` are auto-registered. Error messages use enum types in `Domain/Enumerators/` (e.g., `ValidationErrorMessage`, `BusinessErrorMessage`).

**DTOs:** Inbound and outbound shapes are in `API.Public/DTOs/`. Entities have a `WithoutRelations()` method for selective mapping.

## Infrastructure

- **Database:** SQL Server via EF Core 10. Dev uses LocalDB (`RECYCLY_DEV`). Connection string goes in `appsettings.Development.json`.
- **Auth:** JWT bearer tokens (symmetric key) + refresh token rotation. Settings under `Settings.JwtSettings` and `Settings.AuthSettings` in appsettings.
- **Background jobs:** Hangfire with SQL Server storage. `ITrackingCodeEmailJob` sends order tracking emails. Dashboard at `/hangfire`.
- **Real-time:** SignalR hub at `/hubs/notifications` for admin notifications (`IAdminNotificationService`).
- **Email:** Resend SDK via `IEmailService`.
- **Payments:** Generic `IPaymentService`/`PaymentService` — no external payment gateway wired in yet. `CreatePaymentRequest` is the inbound model; status updates go through `UpdatePaymentStatusAsync`.
- **Shipping:** `IShippingService` wraps an external carrier API.
- **File storage:** `IFileStorageService` handles product images and avatars.

## Configuration

All runtime settings are bound to a `Settings` object from appsettings. Secrets and connection strings belong in `appsettings.Development.json` (git-ignored in production). The app sets `InvariantCulture` globally so decimal separators are always `.`.

CORS allows `localhost:5173` and `localhost:5174` (Vite dev server).
