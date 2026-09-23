# DataArc Orchestration Framework Demo

> A .NET 10 WebAPI demo showing explicit application orchestration, policy-driven workflows, high-performance EF Core execution, and multi-`DbContext` SQL Server composition.

**DataArc Orchestration Framework** gives application workflows a clear orchestration layer without replacing ASP.NET Core or Entity Framework Core.

This demo uses a human-resources scenario to show two complementary paths:

- a high-volume employee import using the free `DataArc.EntityFrameworkCore` execution API
- an employee onboarding workflow coordinated through DataArc orchestration and application policies

The solution also includes SQL Server integration tests that compose multiple isolated `DbContext` models into one physical relational database through the commercial `DataArc.EntityFrameworkCore.SqlServer` DDL Builder.

---

## What This Demo Shows

The demo focuses on a small set of concrete ideas:

1. **Explicit orchestration**
   - API endpoints delegate use-case coordination to named orchestrators.
   - Workflow logic does not live in controllers.

2. **Policy-driven application flow**
   - Policies decide whether a workflow may continue.
   - The orchestration layer coordinates the result.

3. **Normal EF Core remains normal EF Core**
   - Persistence uses standard `IDbContextFactory<TContext>`.
   - Queries use ordinary EF Core APIs.
   - DataArc is used only where additional execution behavior is required.

4. **High-volume bulk execution**
   - The HR import endpoint uses `AsParallel()`, `AddBulk(...)`, and `SaveChangesParallelAsync()`.

5. **Modular persistence boundaries**
   - HR, Finance, IT, and Operations each own a separate `DbContext`.
   - The contexts remain independently usable.

6. **One physical relational database**
   - The demo maps the four contexts into the same SQL Server database.
   - The commercial DDL Builder composes the models into one database definition.

7. **No-migrations DDL integration test**
   - The integration test can generate, drop, and recreate the composed database directly from the participating `DbContext` models.

The core idea is simple:

```text
ASP.NET Core receives the request.
Application services expose the use case.
Orchestrators coordinate the workflow.
Policies decide whether work may continue.
EF Core owns persistence.
DataArc adds orchestration and execution where required.
```

---

## Package Model

The demo intentionally separates the free orchestration stack from the commercial SQL Server tooling.

### DataArc.OrchestrationFramework

`DataArc.OrchestrationFramework` is the top-level orchestration package used by the application/orchestration layer.

Version `2.0.0` brings the free DataArc stack required by the demo, including:

- `DataArc.Orchestrator`
- `DataArc.Observer`
- `DataArc.EntityFrameworkCore`

A project that only needs the free orchestration and EF Core execution capabilities can reference:

```xml
<PackageReference Include="DataArc.OrchestrationFramework" Version="2.0.0" />
```

### DataArc.EntityFrameworkCore.SqlServer

`DataArc.EntityFrameworkCore.SqlServer` contains the commercial SQL Server-specific capabilities used by this repository's integration tests, including the multi-context DDL Builder and advanced SQL Server transaction execution.

The persistence/integration-test surface references:

```xml
<PackageReference Include="DataArc.EntityFrameworkCore.SqlServer" Version="2.0.0" />
```

`DataArc.EntityFrameworkCore.SqlServer` carries `DataArc.EntityFrameworkCore` as a dependency.

---

## Demo Topology

The demo contains four persistence modules:

```text
HR
Finance
IT
Operations
```

Each module has its own `DbContext`:

```text
HrDbContext
FinanceDbContext
ItDbContext
OperationsDbContext
```

All four contexts target the same physical SQL Server database:

```text
DataArcDemoDb
```

This is deliberate.

A `DbContext` boundary does not have to become a relational boundary.

The contexts remain isolated application/persistence boundaries while the physical database can still be composed as one relational model.

---

## Domain Scenario

The demo models employee import and onboarding.

The onboarding workflow works across records owned by the four modules:

```text
Employee          -> HR
PayrollRecord     -> Finance
AccessRequest     -> IT
OnboardingTask    -> Operations
```

`EmployeeId` is used as the cross-module correlation key.

Conceptually:

```text
HR owns the employee.
Finance owns payroll data.
IT owns access requests.
Operations owns onboarding tasks.
The orchestrator coordinates the use case.
```

---

## HR Import Flow

The import endpoint creates demo HR data and persists it through the free DataArc EF Core execution API.

Endpoint:

```text
POST /api/hr/imports
```

The current orchestration path uses a normal EF Core factory-created `DbContext`:

```csharp
public override async Task<ImportEmployeesOutput> ExecuteAsync(
    ImportEmployeesInput input,
    ImportEmployeesOutput output)
{
    await using var dbContext =
        await _hrDbContextFactory.CreateDbContextAsync();

    var importData = SeedDataGenerator
        .GenerateHrSeedData(input.ImportEmployeeCount);

    await dbContext.AsParallel()
        .AddBulk(importData, input.ImportBatchSize)
        .SaveChangesParallelAsync();

    output.TotalRecordsProcessed = importData.Count;

    return output;
}
```

A successful import of 100,000 records returns:

```json
{
  "totalRecordsProcessed": 100000,
  "errors": []
}
```

The important boundary is:

```text
IDbContextFactory<HrDbContext>
        |
        v
normal HrDbContext
        |
        v
DataArc AsParallel()
        |
        v
bulk execution
```

DataArc does not replace the `DbContext`.

---

## Employee Onboarding Flow

The onboarding endpoint coordinates the application workflow across HR, Finance, IT, and Operations.

Example endpoint:

```text
POST /api/hr/onboarding
```

Example request:

```json
{
  "employeeId": 1,
  "annualSalary": 85000,
  "currencyCode": "USD",
  "reason": "Demo employee onboarding"
}
```

The workflow is intentionally explicit:

```text
Load current onboarding state.
Apply the onboarding policy.
Reject invalid or duplicate onboarding.
Create the required module-owned records.
Update the employee onboarding state.
Return a structured orchestration result.
```

The controller/API endpoint does not own that workflow. It delegates to the application service and orchestration layer.

---

## Multi-Context DDL Builder

This repository is also the natural demo for DataArc's multi-`DbContext` DDL Builder.

The SQL Server integration test composes the four isolated EF Core models into one database definition:

```csharp
var databaseBuilder = _databaseFactory.CreateDatabaseBuilder();

var demoDatabase = databaseBuilder
    .IncludeDbContext<HrDbContext>()
    .IncludeDbContext<ItDbContext>()
    .IncludeDbContext<OperationsDbContext>()
    .IncludeDbContext<FinanceDbContext>()
    .Build(
        generateScripts: true,
        applyChanges: true);

demoDatabase.ExecuteDrop();
demoDatabase.ExecuteCreate();
```

This demonstrates that modular `DbContext` boundaries can participate in one physical relational database without requiring the application to collapse those contexts into one large context.

The integration test deliberately exercises the commercial `DataArc.EntityFrameworkCore.SqlServer` package.

---

## Why The DDL Builder Matters Here

The demo has a topology where the DDL Builder is useful rather than artificial:

```text
HrDbContext
FinanceDbContext
ItDbContext
OperationsDbContext
        |
        +----> one composed SQL Server database
```

That allows the application to preserve module ownership while still using normal relational database capabilities where appropriate.

The DDL Builder can generate and apply the database definition from the participating EF Core models without making EF migrations the only database-construction path for the demo.

---

## EF Core Registration

The demo uses `IDbContextFactory<TContext>` for persistence.

Because DataArc infrastructure is scoped in the WebAPI, the factories are registered with a scoped lifetime as well:

```csharp
services.AddDbContextFactory<HrDbContext>(
    options =>
        options
            .UseSqlServer(
                configurationManager.GetConnectionString("DataArcDemoDb"))
            .UseLoggerFactory(factory),
    ServiceLifetime.Scoped);
```

The same pattern is used for:

```text
FinanceDbContext
HrDbContext
ItDbContext
OperationsDbContext
```

This is important in an ASP.NET Core host where factory-created contexts participate in DataArc execution that resolves scoped infrastructure.

---

## DataArc Registration

The WebAPI owns the DataArc bootstrap.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataArcCore();

builder.Services.AddHRModule(builder.Configuration);
```

`AddDataArcCore()` is registered once at the application composition root.

Feature/module registration methods register their own application, orchestration, and persistence services; they do not own the application-level DataArc bootstrap.

---

## Free And Commercial Paths

The demo intentionally exercises both package boundaries.

### Free path

The WebAPI import flow uses the free EF Core package:

```csharp
await dbContext.AsParallel()
    .AddBulk(importData, input.ImportBatchSize)
    .SaveChangesParallelAsync();
```

No commercial SQL Server transaction or DDL API is required for that path.

### Commercial SQL Server path

The integration-test project uses `DataArc.EntityFrameworkCore.SqlServer` for SQL Server-specific commercial capabilities such as multi-context DDL composition.

Commercial test composition can use a configured key or server-key path, depending on the environment.

---

## Project Structure

The solution separates application contracts, orchestration, persistence, presentation, and integration tests.

```text
DataArc.Orchestration.Framework
|
├── Demo.Application.Domain
│   └── Domain models and domain-facing concepts
|
├── Demo.Application.Domain.SharedKernel
│   └── Shared domain primitives and common domain types
|
├── Demo.Application.Features
│   └── Feature-level contracts and service boundaries
|
├── Demo.Application.Modules
│   └── Application module wiring
|
├── Demo.Orchestration
│   └── Concrete orchestrators and workflow coordination
|
├── Demo.Persistence
│   └── EF Core DbContexts, database setup, and persistence registration
|
├── Demo.WebApi
│   └── ASP.NET Core API entry point and Swagger surface
|
└── Demo.Integration.Tests
    └── SQL Server integration tests, including multi-context DDL generation
```

The structure is intentionally more explicit than a tiny sample because the repository is meant to show where orchestration fits in a real application shape.

---

## Running The Demo

### Requirements

- .NET 10 SDK
- SQL Server, SQL Server Developer Edition, or another SQL Server instance you control

The demo itself targets .NET 10.

### 1. Configure the connection string

Update the WebAPI `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DataArcDemoDb": "Server=YOUR_SERVER;Database=DataArcDemoDb;Integrated Security=true;TrustServerCertificate=True;"
  }
}
```

### 2. Run the WebAPI

Set the startup project to:

```text
Demo.WebApi
```

or run the project from the command line using the repository's WebAPI project path.

### 3. Open Swagger

Use the Swagger URL printed by ASP.NET Core, typically:

```text
https://localhost:PORT/swagger
```

### 4. Import demo HR data

Call:

```text
POST /api/hr/imports
```

A normal demo run imports 100,000 records.

### 5. Run employee onboarding

Call:

```text
POST /api/hr/onboarding
```

Use an employee created by the import flow and inspect the structured orchestration response.

---

## Running The Integration Tests

The integration-test project exercises the SQL Server-specific package surface.

The database setup test demonstrates multi-context DDL composition without EF migrations:

```text
HrDbContext
ItDbContext
OperationsDbContext
FinanceDbContext
        |
        v
DataArc database definition
        |
        v
generate / apply / drop / recreate
```

The tests require access to the configured SQL Server instance and, where a commercial capability is being exercised, a valid DataArc commercial/server-key configuration.

---

## Design Boundaries

The demo follows a few deliberate boundaries.

### ASP.NET Core owns the API surface

Endpoints receive requests and return responses.

### Orchestrators own workflow coordination

The orchestration layer expresses the use case and coordinates participating application boundaries.

### Policies own business decisions

Policies answer whether the workflow may continue.

### EF Core owns persistence

The application still uses normal EF Core `DbContext` models and `IDbContextFactory<TContext>`.

### DataArc adds execution and composition

The free package adds orchestration-friendly EF Core execution such as bulk and parallel execution.

The SQL Server package adds SQL Server-specific commercial capabilities such as advanced transactional execution and multi-context DDL composition.

---

## Boundary Note

DataArc Orchestration Framework is intended for controlled application workflows where the application is allowed to coordinate participating modules and persistence boundaries.

It is not intended to bypass service ownership rules or encourage unrelated services to read and write each other's private databases.

The goal is explicit orchestration, not hidden coupling.

---

## Summary

This demo shows the current DataArc 2.0 story:

- named application orchestration
- policy-driven workflow decisions
- thin ASP.NET Core endpoints
- normal EF Core `DbContext` usage
- `IDbContextFactory<TContext>` persistence
- free parallel/bulk EF Core execution
- modular HR, Finance, IT, and Operations contexts
- one physical SQL Server database
- commercial multi-context DDL composition
- integration tests that exercise the actual package boundaries

The central idea remains:

> Keep application workflows explicit, keep EF Core familiar, and add DataArc only where orchestration, execution, or database composition provides value.

---

Learn more: [www.dataarc.dev](https://www.dataarc.dev)
