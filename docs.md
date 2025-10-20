# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

This is a **Web Restaurant Booking** system built as a .NET 9 microservices architecture using **Clean Architecture** principles. The project consists of separate services for different business domains, each following **Domain-Driven Design (DDD)** patterns with **CQRS** implementation using MediatR.

## Architecture

### High-Level Structure
The system follows a **microservices architecture** with each service implementing **Clean Architecture** layers:

```
Be_Web_Restaurant/BeWebRestaurant/
├── Src/
│   ├── Services/                    # Business microservices
│   │   ├── MenuService/            # Food menu and food types management
│   │   └── InventoryService/       # Stock, ingredients, and recipe management
│   ├── Shared/                     # Cross-cutting concerns
│   │   ├── Common/                 # Shared utilities, behaviors, middleware
│   │   ├── Domain.Core/            # Base domain entities, rules, events
│   │   └── Application.Core/       # Shared application patterns (stub)
│   ├── Tests/                      # Unit tests
│   ├── ApiGateway/                 # API Gateway (planned)
│   └── Bff/                        # Backend for Frontend (planned)
```

### Service Architecture Pattern
Each service follows **Clean Architecture** with four layers:

1. **Domain Layer** - Core business logic, entities, value objects, domain events, enums
2. **Application Layer** - Use cases, CQRS commands/queries, DTOs, mapping, MediatR handlers
3. **Infrastructure Layer** - Data persistence (EF Core), repositories, external services
4. **API Layer** - REST controllers, middleware, DI configuration, Swagger documentation

### Current Services

#### MenuService
**Manages food menu and categorization**
- **Modules**: Food, FoodType
- **Operations**: Full CRUD for menu items and food categories
- **Features**: Comprehensive logging with Serilog, validation pipeline
- **Database**: MenuDatabase (separate SQL Server instance)

#### InventoryService  
**Manages kitchen inventory and recipe management**
- **Modules**: 
  - **Stock**: General stock management
  - **StockItems**: Individual stock item operations (including decrease/consume operations)
  - **Ingredients**: Recipe ingredients management
  - **FoodRecipe**: Recipe creation and ingredient relationships
- **Operations**: Full CRUD plus specialized decrease operations for stock consumption
- **Features**: Transaction-based operations, comprehensive error handling and logging
- **Database**: InventoryDatabase (separate SQL Server instance)

### Key Patterns Implemented

- **Domain-Driven Design (DDD)** - Aggregate roots, entities, value objects, domain events
- **CQRS with MediatR** - Command/Query separation using MediatR request/response pipeline
- **Repository Pattern with Unit of Work** - Data access abstraction with transaction management
- **Clean Architecture** - Dependency inversion, separation of concerns
- **Pipeline Behaviors** - Cross-cutting validation using MediatR pipeline behaviors
- **Command Handler Pattern** - All commands handled by dedicated `*CommandHandler` classes

## Development Commands

### Building and Running

**Build entire solution:**
```bash
cd Be_Web_Restaurant/BeWebRestaurant
dotnet build BeWebRestaurant.sln
```

**Run specific services:**
```bash
# Menu Service (default port: configured in appsettings)
cd Src/Services/MenuService/Menu.Api
dotnet run

# Inventory Service (default port: configured in appsettings)  
cd Src/Services/InventoryService/Inventory.Api
dotnet run
```

**Build and run all services:**
```bash
dotnet build BeWebRestaurant.sln --configuration Release
```

### Database Operations

**Entity Framework migrations (per service):**
```bash
# Menu Service migrations
cd Src/Services/MenuService/Menu.Api
dotnet ef migrations add MigrationName --context MenuDbContext
dotnet ef database update --context MenuDbContext

# Inventory Service migrations  
cd Src/Services/InventoryService/Inventory.Api
dotnet ef migrations add MigrationName --context InventoryDbContext
dotnet ef database update --context InventoryDbContext
```

### Testing

**Run all unit tests:**
```bash
dotnet test BeWebRestaurant.sln
```

**Run tests for specific service:**
```bash
cd Src/Tests/MenuService.UnitTests
dotnet test
```

**Run tests with coverage:**
```bash
dotnet test BeWebRestaurant.sln --collect:"XPlat Code Coverage"
```

### Package Management

**Add package to specific project:**
```bash
cd Src/Services/MenuService/Menu.Api
dotnet add package PackageName
```

**Restore packages:**
```bash
dotnet restore BeWebRestaurant.sln
```

## Current Implementation Status

### Completed Features

#### Shared Infrastructure
- ✅ **Domain.Core**: Base entity, aggregate root, domain events, rule engine, business rule exceptions
- ✅ **Common**: MediatR validation behaviors, middleware, shared DTOs (Money, Measurement), property converters
- ✅ **CQRS Pipeline**: MediatR request/response pattern with validation pipeline
- ✅ **Business Rules Engine**: Comprehensive rule validation with field-specific error handling

#### MenuService - COMPLETED
- ✅ **Food Management**: Full CRUD operations with domain validation
- ✅ **FoodType Management**: Category management with hierarchical support
- ✅ **Advanced Logging**: Serilog with JSON output, console/file logging, TraceId enrichment
- ✅ **Database**: EF Core 9 with SQL Server, migrations, entity configurations
- ✅ **API Documentation**: Swagger/OpenAPI 3.0 integration

#### InventoryService - COMPLETED
- ✅ **Stock Management**: Complete inventory tracking system
- ✅ **StockItems Management**: Individual item operations including specialized decrease/consume operations
- ✅ **Ingredients Management**: Recipe ingredient definitions and management
- ✅ **FoodRecipe Management**: Recipe creation with ingredient relationships and measurements
- ✅ **Transaction Management**: All operations wrapped in database transactions
- ✅ **Error Handling**: Comprehensive logging and business rule validation
- ✅ **Command Handlers**: All handlers follow `*CommandHandler` naming convention

### Architecture Patterns Implemented

#### CQRS Implementation
- **Commands**: Create, Update, Delete operations via MediatR
- **Queries**: Read operations with optimized DTOs (planned for future implementation)
- **Handlers**: Dedicated command handlers with transaction management
- **Validation**: Pipeline behaviors for cross-cutting validation

#### Domain-Driven Design
- **Entities**: Rich domain models with business logic
- **Value Objects**: Money, Measurement, and other immutable types  
- **Aggregates**: Transaction boundaries and consistency rules
- **Domain Events**: Event-driven communication (infrastructure ready)

#### Clean Architecture Compliance
- **Domain Layer**: No external dependencies, pure business logic
- **Application Layer**: MediatR, DTOs, mapping extensions, business use cases
- **Infrastructure Layer**: EF Core, SQL Server, external service implementations
- **API Layer**: ASP.NET Core controllers, dependency injection, middleware

## Technology Stack

### Core Technologies
- **.NET 9** - Latest .NET framework with C# 13
- **Entity Framework Core 9** - ORM with SQL Server provider
- **MediatR** - CQRS and messaging patterns implementation
- **SQL Server** - Database engine (separate DBs per service)

### Logging & Monitoring
- **Serilog** - Structured logging with JSON output
- **Serilog.AspNetCore** - ASP.NET Core integration
- **Serilog.Enrichers.Activity** - TraceId enrichment for request correlation

### API & Documentation
- **Swagger/OpenAPI 3.0** - API documentation and testing interface
- **Microsoft.AspNetCore.OpenApi** - OpenAPI specification generation

### Development & Testing
- **xUnit** - Unit testing framework
- **Microsoft.EntityFrameworkCore.Design** - EF Core tooling for migrations

### Database Configuration
Each service maintains its own database:
- **MenuService**: `MenuDatabase` connection string in `appsettings.Development.json`
- **InventoryService**: `InventoryDatabase` connection string in `appsettings.Development.json`

### Logging Configuration
Both services have comprehensive Serilog configuration with:
- Console and file output for development
- JSON structured logs for production consumption
- TraceId enrichment for request correlation across services
- Rolling daily logs with 30-day retention
- Separate log directories: `logs/be/` (backend) and `logs/fe/` (frontend logs if needed)

## Development Guidelines

### Adding New Services
1. Follow the established Clean Architecture structure (4 layers)
2. Create Domain, Application, Infrastructure, and API projects
3. Add appropriate project references in `BeWebRestaurant.sln`
4. Implement Repository pattern with Unit of Work for data access
5. Configure MediatR and validation behaviors in Program.cs
6. Add service-specific DbContext and configure connection strings
7. Create initial EF migrations

### Adding New Features
1. **Start with Domain**: Create entities, value objects, and domain events in Domain layer
2. **Application Layer**: Create CQRS command/query classes and DTOs
3. **Implement Handlers**: Use MediatR `*CommandHandler` naming convention
4. **Repository Pattern**: Add interfaces and implementations following existing patterns  
5. **API Controllers**: Create REST endpoints with proper error handling
6. **Database Changes**: Always create EF migrations and test in development
7. **Unit Tests**: Write tests covering domain logic and business rules

### Command Handler Pattern
All command handlers follow this standardized pattern:
- **Naming**: `*CommandHandler` (e.g., `CreateStockCommandHandler`)
- **Logging**: LogInformation on start/success, LogWarning on business rule failures, LogError on exceptions
- **Transactions**: All business operations wrapped in database transactions
- **Error Handling**: Separate catch blocks for BusinessRuleException and general exceptions

### Business Rules Implementation
- Use `Domain.Core.Rule.RuleFactory` for creating business rule exceptions
- Implement `IValidateRequest` on command classes for pipeline validation
- Use field-specific error codes from `*FieldNames` classes
- Return structured error information with parameter details

### Database Schema Changes
- Each service manages its own database schema independently
- Always create EF migrations when making schema changes
- Test migrations thoroughly in development before applying to other environments
- Use appropriate entity configurations for complex relationships

### Future Planned Services
The architecture supports easy addition of new services such as:
- **BookingService** - Table reservations and customer management
- **OrderService** - Order processing and kitchen workflow
- **PaymentService** - Payment processing integration
- **UserService** - Authentication and user management
- **NotificationService** - Email/SMS notifications

### API Gateway & BFF
The solution structure includes placeholders for:
- **ApiGateway** - Centralized entry point for client requests
- **BFF (Backend for Frontend)** - Aggregated APIs optimized for specific frontend needs

This architecture provides a solid foundation for a scalable restaurant booking system with clear separation of concerns and enterprise-grade patterns.
