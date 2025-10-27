# Context Map

Bounded Contexts
- MenuService (Menu BC): Manages menu, foods, food types, pricing and status.
- InventoryService (Inventory BC): Manages stock, stock items, ingredients, and recipes (food-to-ingredients).
- AuthService (Auth BC - planned): Authentication/authorization, user/roles; currently a stub.

Relationships and Integration
- Menu <- Inventory: Menu queries Inventory for availability/recipe info to decide visibility or status changes (future). Integration via async domain/integration events.
- Anti-Corruption Layer (ACL): Each BC exposes its own DTOs/contracts; translation occurs in Application/Infrastructure mappers. No shared domain types across BCs.

Integration Style (target)
- In-process per service: MediatR publishes domain events internally (implemented).
- Cross-service: Outbox + background publisher to emit integration events (planned). Subscribers translate into local concepts through ACL mappers.

Contracts
- Event names: <Entity><Created|Updated|Deleted> (standardized across BCs).
- Message shape: minimal, stable identifiers + value data. Versioned if needed.

Conformance
- No direct references between Menu.Domain and Inventory.Domain.
- APIs depend only on their Application; Infrastructure registered via DI extension.
