# Dependency Rule 1.1

Goal: Within each bounded context, keep dependencies directed inward and keep the API thin:
- Domain -> Shared Domain.Core only
- Application -> Domain
- Infrastructure -> Application (implements contracts)
- API -> Application (+ Api.Core); API must not construct Domain types and should register Infrastructure via DI only

## What changed and why

1) Menu.Api decoupled from Domain and direct Infrastructure wiring
- Changed `Menu.Api/Controllers/FoodController.cs`
  - Before: controller constructed `FoodStatus` (Domain VO) from `FoodStatusEnum` and sent it via MediatR.
  - After: controller accepts `[FromRoute] string status` and sends a primitive to the Application layer. No Domain usings remain in controller.
  - Why: API is an outer layer; constructing Domain types here increases coupling and leaks Domain concerns. Passing primitives keeps the boundary clean; Application translates to Domain.

- Changed `Menu.Application/Modules/Food/Queries/GetFoodByStatus/GetFoodByStatusQuery.cs`
  - Before: `GetFoodByStatusQuery(FoodStatus foodStatus)` required a Domain VO.
  - After: `GetFoodByStatusQuery(string Status)` uses a primitive at the application boundary.
  - Why: Application contracts should be simple and not force API to reference Domain types.

- Changed `Menu.Application/Modules/Food/Queries/GetFoodByStatus/GetFoodByStatusQHandler.cs`
  - Now parses `query.Status` to `FoodStatusEnum` and creates `FoodStatus` inside the handler, then calls repository.
  - Also passes `CancellationToken` to repository calls.
  - Why: Domain translation happens inside Application, preserving layering and enabling validation/mapping in one place.

- Changed `Menu.Api/Menu.Api.csproj`
  - Removed reference to `Menu.Domain`. Kept references to `Menu.Application` and `Menu.Infrastructure`.
  - Why: API must not depend on Domain. Infrastructure reference is used only to call a DI extension (acceptable composition root pattern). If you want zero compile-time Infra reference, move DI to a host/bootstrap project.

- Changed `Menu.Api/Program.cs`
  - Before: registered `MenuDbContext`, repositories, and UoW directly; imported Infra types.
  - After: calls `builder.Services.AddMenuInfrastructure(builder.Configuration);` and removes direct Infra type usings.
  - Why: Centralize Infra wiring inside Infrastructure assembly; the API composes it without knowing details.

- Added `Menu.Infrastructure/DependencyInjection.cs`
  - New extension: `AddMenuInfrastructure(IServiceCollection, IConfiguration)` registers DbContext, repositories, and UoW.
  - Why: Encapsulates Infrastructure wiring; supports substitutability and testing.

2) Inventory.Api symmetrical cleanup
- Added `Inventory.Infrastructure/DependencyInjection.cs` with `AddInventoryInfrastructure` to register DbContext, repositories, and UoW.
- Changed `Inventory.Api/Program.cs` to use `AddInventoryInfrastructure` and removed direct Infra registrations/usings.
- Changed `Inventory.Api/Inventory.Api.csproj` to remove reference to `Inventory.Domain` (API now references only Application + Infrastructure + Api.Core).
- Why: Apply the same dependency boundary: API does not depend on Domain and does not wire Infra directly.

## Resulting dependency structure (per bounded context)
- Domain: depends only on `Shared/Domain.Core`
- Application: depends on Domain; exposes commands/queries that accept primitives/DTOs
- Infrastructure: depends on Application (implements repository contracts) and registers itself via a DI extension
- API: depends on Application (and Api.Core); references Infrastructure only to call a DI extension, and never constructs Domain objects

## Notes and next steps
- If you want the API to have zero compile-time reference to Infrastructure, move the DI invocation to a Host/Bootstrap project or use assembly scanning/plugins. Current approach keeps API ignorant of Infra details while allowing simple registration.
- Handlers are now the single place where mapping from primitives to Domain value objects occurs, making validation and rule enforcement consistent.

# 1.2 Domain Layer (Core Business Logic)

Goal: Aggregates encapsulate behavior and invariants. Value Objects centralize validation and are immutable. Domain Events capture business-relevant changes so other bounded contexts/integrations can react when we add dispatching later.

## What changed and why

1) Standardized domain events for Food aggregate (create, update, delete)
- Added `Menu.Domain/Events/FoodEvents/FoodUpdatedEvent.cs`
  - Generic update event for non-status changes (e.g., name, image, description, money, food type).
  - Why: We already had a status-specific event; this covers other updates without proliferating event types.

- Added `Menu.Domain/Events/FoodEvents/FoodDeletedEvent.cs`
  - Raised when a Food is deleted.
  - Why: Consumers may need to purge caches or synchronize read models on deletion.

2) Raised events from aggregate behaviors (MenuService)
- Updated `Menu.Domain/Entities/Food.cs`
  - `UpdateBasic(...)` now raises `FoodUpdatedEvent(Id, UpdatedAt)`.
  - `UpdateMoney(...)` now raises `FoodUpdatedEvent`.
  - `UpdateFoodTypeId(...)` now raises `FoodUpdatedEvent`.
  - `UpdateStatus(...)` continues to raise `FoodUpdatedStatusEvent`.
  - Added `Delete()` to raise `FoodDeletedEvent(Id, UpdatedAt)`.
- Updated `Menu.Domain/Entities/FoodType.cs`
  - `Create(...)` now raises `FoodTypeCreatedEvent`.
  - `UpdateName(...)` now raises `FoodTypeUpdatedEvent`.
  - Added `Delete()` to raise `FoodTypeDeletedEvent`.
  - Added private `Touch()` to normalize timestamp updates.
  - Why: Events originate inside the aggregate to ensure invariants before publishing changes.

3) InventoryService: create/update/delete events for all entities
- Added events under `Inventory.Domain/Events/InventoryEvents/`:
  - Stock: `StockCreatedEvent`, `StockUpdatedEvent`, `StockDeletedEvent`
  - StockItems: `StockItemsCreatedEvent`, `StockItemsUpdatedEvent`, `StockItemsDeletedEvent` (kept existing `StockItemsUpdateStatusEvent`)
  - Ingredients: `IngredientsCreatedEvent`, `IngredientsUpdatedEvent`, `IngredientsDeletedEvent`
  - FoodRecipe: `FoodRecipeCreatedEvent`, `FoodRecipeUpdatedEvent`, `FoodRecipeDeletedEvent`
- Updated aggregates to raise events:
  - `Stock.Create(...)` -> created; `Update(...)` -> updated; `Delete()` -> deleted.
  - `StockItems.Create(...)` -> created; all mutators (`Increase/Decrease/UpdateMeasurement`, `UpdateCapacity`, `UpdateStatus`, `UpdateIngredientsId`, `UpdateStockId`) now raise generic updated after `Touch()`; `UpdateStatus` also raises status-specific event; added `Delete()`.
  - `Ingredients.Create(...)` -> created; `Update(...)` -> updated; `Delete()` -> deleted; added `Touch()`.
  - `FoodRecipe.Create(...)` -> created; `Update(...)` -> updated; `Delete()` -> deleted; added `Touch()`.
- Why: Standardized event set (Created/Updated/Deleted) across entities supports outbox/integration later and consistent audit trails.

4) No dispatch infrastructure yet (by design)
- We only define event types and raise them via `AddDomainEvent(...)`. Actual dispatch/handlers will be added later at the Application/Infrastructure boundary (e.g., outbox pattern or MediatR notifications).
- Why: Aligns with the plan to add event handling later without blocking functional work now.

## Impact and usage guidance
- Application layer continues to orchestrate use cases; repositories persist aggregates as before.
- When deleting a Food, application code can call `food.Delete()` prior to repository deletion to enqueue the domain event.
- For other aggregates (e.g., Inventory's `StockItems`), mirror this pattern: introduce Created/Updated/Deleted events and raise them from behaviors enforcing invariants.

# 1.3 Application Layer (Use Cases)

Goal: Express use cases with MediatR; keep DTOs out of the domain; centralize validation; orchestrate repositories and aggregates; avoid leaking infrastructure concerns into domain.

## Review outcome
- Commands/queries: Clear, thin records with mapping in Application layer. OK.
- Validation: Centralized via `ValidationBehaviors<TReq,TRes>` and `IValidateRequest.GetRule()` using RuleFactories. OK.
- DTO mapping: Requests map to domain via extension methods (e.g., `Request.ToFood()`); responses map from domain. OK.
- Transactions: Handlers open/commit/rollback through `IUnitOfWork`. Acceptable for now.
- Duplication checks: Handlers check uniqueness/existence via repositories. Acceptable; can be promoted to DB unique constraints or domain specifications later.
- Dependency rule: Queries accept primitives at the boundary (e.g., `GetFoodByStatusQuery(string Status)`); handler translates to VOs. OK.

## Changes made (and why)
1) Boundary primitives in queries
- Updated `Menu.Application` GetFoodByStatus to accept `string Status`, moving VO creation to its handler.
- Why: Keeps Application boundary simple and decouples API from Domain types.

2) Consistent cancellation
- Ensured handlers and repositories propagate `CancellationToken` in updated code paths (e.g., Menu GetFoodByStatus, Inventory GetStockItemsByStatus).
- Why: Cooperative cancellation across layers.

3) Inventory: boundary primitive for status query
- Updated `Inventory.Application` GetStockItemsByStatus to accept `string Status`; handler parses to `StockItemsStatusEnum` and creates VO.
- Updated `Inventory.Api` StockItemsController to accept `string status` and removed Domain enum from API.
- Why: Keep API and Application boundaries free of Domain types; translate inside handlers.

## Improvements to consider (not applied now)
- Transaction pipeline behavior: Introduce `TransactionBehavior<TReq,TRes>` to wrap all handlers, removing per-handler transaction boilerplate; register via MediatR pipeline. Current explicit pattern is OK but repetitive.
- Specifications/Policies: Move duplicate/existence checks behind named specs for reuse; or enforce via DB constraints and handle violations.
- Query shaping: For joins like Food + FoodType in queries, consider repo methods that project directly to DTO to reduce in-memory joins if needed.

# 1.4 Infrastructure Layer

Goal: Implement repositories and persistence concerns without leaking into Domain/Application; add event dispatch so domain events leave the aggregate boundary.

## What changed and why
1) Domain event dispatch in UnitOfWork (both services)
- Menu.Infrastructure/Persistence/UnitOfWork.cs
  - Injected IMediator and, after SaveChangesAsync, dispatches all AggregateRoot.DomainEvents via MediatR and then clears them; commits transaction after dispatch.
- Inventory.Infrastructure/Persistence/UnitOfWork.cs
  - Same pattern: SaveChangesAsync -> dispatch events -> commit; rollback unchanged.
- Why: Ensures side effects are published post-persistence; keeps dispatch at the infrastructure boundary.

2) DI remains simple
- DependencyInjection in both services still registers UnitOfWork and repositories; MediatR is registered at API, so UnitOfWork can resolve IMediator.
- Why: Composition root stays in API; Infrastructure only exposes AddXInfrastructure.

## Next steps (optional, not implemented now)
- Outbox pattern: persist domain events to an Outbox table in the same transaction, and process via BackgroundService for durability and cross-service integration.
- SaveChanges interceptor alternative: move event collection/dispatch to EF Core interceptor to decouple from UoW if desired.
- Telemetry: log published domain events (type, aggregate id) for observability.

# 1.5 Presentation Layer

Goal: Keep controllers transport-focused and thin; no domain construction in controllers; delegate to MediatR; rely on global exception middleware for consistent HTTP mapping.

## What changed and why
1) Removed domain leakage from status endpoints
- Menu.Api/FoodController.GetByStatus: controller now accepts `string status` and sends `GetFoodByStatusQuery(status)`; the handler parses and creates the VO.
- Inventory.Api/StockItemsController.GetByStockItemsStatus: controller now accepts `string status` and sends `GetStockItemsByStatusQuery(status)`; the handler handles VO creation.
- Why: Controllers should not construct Domain types; they should pass primitives and let Application map/validate.

2) Consistent cancellation tokens and thin controllers
- All endpoints already pass `CancellationToken` to MediatR; no changes required.
- Controllers remain simple delegate layers with no business logic; rely on ExceptionMiddleware for error mapping.

## Next steps (optional, not implemented now)
- Input normalization: consider adding a small binder/normalizer for status strings (trim/lowercase) before sending to handlers.
- ProblemDetails: ensure ExceptionMiddleware emits RFC7807 ProblemDetails consistently for client UX.

Updates applied
- ProblemDetails: Updated ExceptionMiddleware to return application/problem+json with RFC7807 fields (type, title, status, detail, instance, traceId) and retained error list.
- Input normalization: Trimmed status strings in handlers before enum parsing (Menu GetFoodByStatus, Inventory GetStockItemsByStatus).
- Tests: Added in-memory EF tests to verify domain events are dispatched on CommitAsync (Menu.Domain.Tests, Inventory.Domain.Tests).
- DB constraints: Uniqueness already enforced via EF configurations (Food.FoodName, FoodType.FoodTypeName, Stock.StockName, Ingredients.IngredientsName, composite keys for StockItems and FoodRecipe). No change required.

# 2. DOMAIN-DRIVEN DESIGN

## 2.1 Strategic Design - Bounded Contexts
Score before: 6/10 → Improved by adding documentation.
- Added `docs/context-map.md` describing Menu, Inventory, Auth contexts, relationships, and intended integration via Outbox.
- Reaffirmed boundaries in code: APIs depend on Application only; Infrastructure added via DI extension; no cross-domain references.
- Why: Make boundaries explicit for onboarding and future integrations; prevent leakage across contexts.

## 2.2 Tactical Design - Building Blocks
Score before: 7/10 → Improved by adding dispatching and standardizing events.
- Implemented domain event dispatch in both UnitOfWork classes (Menu/Inventory) via MediatR after SaveChanges.
- Standardized Created/Updated/Deleted events across all aggregates; status-specific events remain.
- Why: Ensures domain changes propagate; sets a foundation for integration/outbox without complicating aggregates.

## 2.3 Ubiquitous Language
Score before: 7/10 → Improved by adding a glossary.
- Added `docs/ubiquitous-language.md` capturing core terms (Food, StockItems, Money, Measurement, etc.) and rules.
- Why: Reduce ambiguity and speed up onboarding.

Notes
- AuthService remains a stub; leave as future work to define its context, contracts, and ACL when requirements are known.
- Outbox is planned (not implemented) for cross-service durability; current in-process dispatch unblocks internal subscribers.

# 3.4 Error Handling & Validation

Status: Solid, with enhancements applied.
- Validation: Pipeline behavior executes `IValidateRequest.GetRule()` before handlers. No changes required.
- ProblemDetails: Updated ExceptionMiddleware to return RFC7807-compliant `application/problem+json` including type, title, status, detail, instance, traceId, and Errors array.
- Input normalization: Trim status strings in handlers before enum parsing to reduce 400s from whitespace/case issues.
- Tests: Added in-memory EF tests exercising domain event dispatch on commit; add more tests as needed for validation rule failures and error mapping.

Why: Consistent error responses improve client UX; normalization avoids brittle routing; tests guard core behaviors.

# 4. Anti-Patterns & Code Smells

Anemic Domain Model
- Detected? No. Aggregates (Food, StockItems, etc.) encapsulate behavior and invariants. No action.

Leaky Abstractions
- Detected? Yes (before). Fix applied:
  - Removed Domain types from controllers and API project references; queries accept primitives and handlers create VOs.
  - APIs still reference Infrastructure only to call DI extensions (acceptable composition root). If zero Infra reference is required, move DI to a host project.
  - Why: Keeps transport layer independent of domain internals and persistence wiring.

Tight Coupling
- Detected? Yes (before). Fix applied:
  - Controllers no longer construct value objects (e.g., FoodStatus); conversion moved into handlers.
  - Why: Future status rule changes won’t force API changes.

Business Logic Leakage
- Detected? Yes (before). Fix applied:
  - UnitOfWork in both services now saves changes and then dispatches accumulated domain events via MediatR; aggregates are cleared afterwards.
  - Why: Ensures side effects propagate to listeners; sets foundation for Outbox.

God Objects/Classes
- Detected? No. No action.
