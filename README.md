# DataArc Orchestration Framework Demo

> **Application orchestration inside the process. Application hosting and observability around it.**

This .NET 10 demo shows how **DataArc** coordinates explicit application workflows across modular EF Core persistence boundaries while **.NET Aspire** hosts and observes the applications that run those workflows.

The distinction is deliberate:

```text
.NET Aspire
    hosts applications
    provides service discovery
    collects logs, health and telemetry

DataArc
    orchestrates application workflows
    evaluates policies
    dispatches application events
    coordinates EF Core work across boundaries
```

The repository uses a human-resources scenario with two visible workflows:

- **Import Employees** — imports 100,000 employee records through DataArc's bulk/parallel EF Core execution path.
- **Employee Onboarding** — coordinates HR, Finance, IT and Operations through an explicit application orchestrator and a single local SQL Server transaction.

The solution also demonstrates **ASP.NET Core Identity**, multiple isolated `DbContext` models, cross-context foreign keys, schema boundaries, and DataArc's SQL Server DDL Builder composing those models into one relational database.

---

## What the demo proves

### Application boundaries do not have to become relational boundaries

The demo keeps independent EF Core contexts for authentication and business modules:

```text
AuthDbContext
HrDbContext
FinanceDbContext
ItDbContext
OperationsDbContext
```

All five participate in one SQL Server database while retaining their own responsibilities.

```text
dbo.AspNetUsers
      │
      ▼
hr.Employees
      │
      ├── hr.EmployeeDepartment ───► hr.Department
      ├── finance.PayrollRecords
      ├── it.AccessRequests
      └── operations.OnboardingTask
```

Schemas remain explicit:

```text
dbo          ASP.NET Core Identity
hr           employees, employers, departments
finance      payroll
it           access requests
operations   onboarding tasks
```

The database remains normalized and database-enforced foreign keys cross context and schema boundaries where the model requires them.

---

## Solution architecture

```text
┌──────────────────────────── .NET Aspire ────────────────────────────┐
│                                                                    │
│   Demo.Host.Aspire.Web ───────────────► Demo.WebApi                │
│        Razor Components                 ASP.NET Core API            │
│              │                                │                    │
│              │                                ▼                    │
│              │                     DataArc application workflow     │
│              │                         Policies / Events            │
│              │                         Orchestration                │
│              │                                │                    │
│              │                                ▼                    │
│              │                    EF Core persistence modules       │
│              │               HR / Finance / IT / Operations        │
│              │                                │                    │
│              └────────────────────────────────┼────────────────────┤
│                                               ▼                    │
│                              SQL Server — one relational database   │
│                                                                    │
│   service discovery • health • logs • traces • telemetry           │
└────────────────────────────────────────────────────────────────────┘
```

### Aspire's responsibility

`.NET Aspire` is the **application/infrastructure orchestration layer** for the demo. The AppHost starts the frontend and API, supplies service discovery, exposes health information, and centralizes development-time logs and telemetry.

### DataArc's responsibility

`DataArc` is the **in-process application orchestration layer**. It coordinates use cases after a request reaches the API. It does not replace Aspire, ASP.NET Core, or EF Core.

That distinction is the central reason Aspire is included in this repository.

---

## Demo workflows

### 1. Import Employees

Open **Import Employees** in the Razor frontend or call:

```text
POST /api/hr/imports
```

The demo generates 100,000 employee records and executes the import through the free DataArc EF Core execution API.

The core execution path remains an ordinary factory-created `DbContext` enhanced with DataArc execution behavior:

```csharp
await using var dbContext =
    await _hrDbContextFactory.CreateDbContextAsync();

await dbContext.AsParallel()
    .AddBulk(importData, input.ImportBatchSize)
    .SaveChangesParallelAsync();
```

The UI reports the result directly, for example:

```text
100,000 employees imported in 1,351 milliseconds.
```

This workflow demonstrates high-volume EF Core execution without replacing `DbContext` or normal EF Core usage.

---

### 2. Employee Onboarding

Open **Onboarding** in the Razor frontend or call:

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
Load the HR employee and organizational context
        ↓
Evaluate the onboarding policy
        ↓
Reject invalid or duplicate onboarding
        ↓
Begin one local SQL Server transaction
        ↓
HR          update employee status
Finance     create payroll record
IT          create access request
Operations  create onboarding task
        ↓
Commit atomically
```

The participating business contexts are:

```text
HrDbContext
FinanceDbContext
ItDbContext
OperationsDbContext
```

All four represent boundaries inside the same physical relational database. The workflow shares the connection and transaction so the onboarding operation commits or rolls back as one local unit of work.

Expected business rejections are returned through the API contract and shown by the UI. For example, attempting to onboard the same employee twice produces a clear rejection rather than an infrastructure failure.

---

## Policy and event flow

The application layer keeps decisions and coordination explicit:

```text
Request
   ↓
Application Service
   ↓
Orchestrator
   ├──► Policy evaluation
   ├──► Persistence coordination
   └──► Application events / observers
   ↓
Structured response
```

The API surface stays thin. Persistence remains in EF Core. Workflow coordination stays in the orchestration layer.

---

## ASP.NET Core Identity composition

Authentication data uses standard ASP.NET Core Identity with an integer key through `AuthDbContext`.

Identity remains in the default `dbo` schema for this release:

```text
dbo.AspNetUsers
dbo.AspNetRoles
dbo.AspNetRoleClaims
dbo.AspNetUserClaims
dbo.AspNetUserLogins
dbo.AspNetUserRoles
dbo.AspNetUserTokens
```

The HR employee is a separate business representation of the same person. A real relational foreign key connects the HR model to Identity:

```text
dbo.AspNetUsers.Id
        │
        ▼
hr.Employees.UserId
```

Organizational membership remains an HR concern:

```text
Employee
    ↓
EmployeeDepartment
    ↓
Department
```

Downstream Finance, IT and Operations records continue to reference `EmployeeId` directly.

---

## DataArc SQL Server DDL Builder

The SQL Server integration surface composes all five EF Core models into one database definition:

```csharp
var demoDatabase = databaseBuilder
    .IncludeDbContext<AuthDbContext>()
    .IncludeDbContext<HrDbContext>()
    .IncludeDbContext<ItDbContext>()
    .IncludeDbContext<OperationsDbContext>()
    .IncludeDbContext<FinanceDbContext>()
    .Build(
        generateScripts: true,
        applyChanges: true);
```

The current demo initialization uses a create-and-reconcile sequence:

```csharp
demoDatabase.ExecuteDrop();

// Create the database, schemas, tables, keys and relationships.
demoDatabase.ExecuteCreate();

// Reconcile the new database against the complete EF Core model metadata.
demoDatabase.ExecuteCreate();
```

The second call runs against the existing database and applies metadata-driven constraints discovered during reconciliation, including Identity uniqueness and the `EmployeeDepartment` composite uniqueness rule.

This keeps the EF Core model authoritative while demonstrating DataArc's database-composition path without requiring EF migrations for the demo database.

---

## Package model

### DataArc.OrchestrationFramework

The application/orchestration layer uses the DataArc orchestration stack:

```xml
<PackageReference Include="DataArc.OrchestrationFramework" Version="2.0.0" />
```

The orchestration framework brings together the free application stack used by the demo, including orchestration, observers/events and EF Core execution capabilities.

### DataArc.EntityFrameworkCore

The employee import demonstrates the free EF Core execution path, including bulk and parallel execution.

### DataArc.EntityFrameworkCore.SqlServer

The commercial SQL Server package is used where SQL Server-specific capabilities are intentionally demonstrated, including:

- multi-`DbContext` DDL composition
- cross-context database construction
- advanced coordinated SQL Server transaction execution

```xml
<PackageReference Include="DataArc.EntityFrameworkCore.SqlServer" Version="2.0.0" />
```

---

## Project structure

```text
DataArc.Orchestration.Framework
│
├── src
│   ├── App
│   │   ├── Demo.Application.Domain
│   │   ├── Demo.Application.Domain.SharedKernel
│   │   ├── Demo.Application.Features
│   │   └── Demo.Application.Modules
│   │
│   ├── Host
│   │   └── Demo.Host.Aspire
│   │       ├── Demo.Host.Aspire.AppHost
│   │       ├── Demo.Host.Aspire.ServiceDefaults
│   │       └── Demo.Host.Aspire.Web
│   │
│   ├── Infrastructure
│   │   └── Demo.Persistence
│   │
│   ├── Orchestration
│   │   └── Demo.Orchestration
│   │
│   └── Presentation
│       └── Demo.WebApi
│
└── tests
    └── Demo.Integration.Tests
```

### Layer responsibilities

| Area | Responsibility |
|---|---|
| `Demo.Application.Domain` | entities, value objects, policies and application events |
| `Demo.Application.Features` | feature contracts and application-facing boundaries |
| `Demo.Application.Modules` | module composition and adapters |
| `Demo.Orchestration` | concrete orchestrators, ports and observers |
| `Demo.Persistence` | EF Core contexts, models and SQL Server persistence |
| `Demo.WebApi` | ASP.NET Core API and Swagger surface |
| `Demo.Host.Aspire.Web` | Razor Components demo UI |
| `Demo.Host.Aspire.AppHost` | Aspire application hosting and service topology |
| `Demo.Integration.Tests` | SQL Server composition and workflow integration verification |

---

## Running the demo

### Requirements

- .NET 10 SDK
- SQL Server / SQL Server Developer Edition
- a local environment capable of running .NET Aspire

The physical demo database is:

```text
DataArcOrchestrationDemo
```

The application connection-string key is:

```text
DataArcDemoDb
```

### 1. Configure SQL Server

Update the connection string used by the API and integration tests for your SQL Server instance.

Example:

```json
{
  "ConnectionStrings": {
    "DataArcDemoDb": "Server=.;Database=DataArcOrchestrationDemo;Integrated Security=SSPI;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

### 2. Run the demo setup/reset script

Before the first walkthrough, run:

```text
scripts/setup.sql
```

The setup/reset script is the explicit entry point for preparing the demo environment. It must not replace the DataArc DDL Builder by hard-coding the complete application schema: DataArc remains responsible for composing the participating EF Core models into the database.

If the seeded employee has already been onboarded and you want to replay the onboarding workflow from its initial state, run the documented reset/setup step again before repeating the scenario.

> The repository setup script is part of the release checklist. If it is not yet present in your checkout, use the integration-test initialization path until that release item has been completed.

### 3. Start the Aspire AppHost

Use `Demo.Host.Aspire.AppHost` as the startup project.

Aspire starts:

```text
apiservice    -> Demo.WebApi
webfrontend   -> Demo.Host.Aspire.Web
```

The frontend references `apiservice` through Aspire service discovery rather than a hard-coded localhost port.

### 4. Use the Razor demo UI

The frontend exposes the two main walkthroughs:

```text
/employees/import    Import Employees
/onboarding          Employee Onboarding
```

### 5. Inspect the API through Swagger

`Demo.WebApi` exposes Swagger in Development. Aspire surfaces the API endpoint in its resource dashboard.

Current demo endpoints:

```text
POST /api/hr/imports
POST /api/hr/onboarding
```

### 6. Inspect logs and telemetry in Aspire

Use the Aspire dashboard to inspect:

- application resources and health
- service startup/order
- API and frontend console logs
- EF Core command output
- request activity and telemetry exposed by Service Defaults

This is useful when comparing **application hosting orchestration** in Aspire with **application workflow orchestration** in DataArc.

---

## Demo / QA license key

The integration-test configuration contains a **QA-generated DataArc key intended for this public demo repository**. It is not a production/customer license and is deliberately supplied so the published demo can exercise the licensed SQL Server surface without documenting internal server-key infrastructure.

---

## Running the integration tests

The integration tests require access to the configured SQL Server instance.

They verify the real package surface rather than mocks, including:

- composed database creation
- ASP.NET Core Identity participation
- cross-context and cross-schema foreign keys
- HR/Department relational membership
- onboarding persistence across HR, Finance, IT and Operations
- coordinated transaction behavior

The database setup sequence deliberately resets the test database so test runs remain deterministic.

---

## Design boundaries

The demo intentionally keeps these responsibilities separate:

**ASP.NET Core** owns HTTP/API concerns.  
**Razor Components** provide the evaluator-facing demo UI.  
**.NET Aspire** owns development-time application hosting, discovery and observability.  
**DataArc Orchestrator** owns application workflow coordination.  
**Policies** own business decisions.  
**EF Core** owns normal persistence and querying.  
**DataArc.EntityFrameworkCore** adds opt-in execution behavior such as bulk/parallel operations.  
**DataArc.EntityFrameworkCore.SqlServer** adds SQL Server-specific commercial capabilities.

The goal is explicit orchestration, not hidden coupling.

---

## Links

- **DataArc:** [https://www.dataarc.dev](https://www.dataarc.dev)
- **GitHub repository:** [SolidArcSoftware/DataArc.Orchestration.Framework](https://github.com/SolidArcSoftware/DataArc.Orchestration.Framework)
- **Solid Arc Software on GitHub:** [https://github.com/SolidArcSoftware](https://github.com/SolidArcSoftware)
- **NuGet — DataArc.OrchestrationFramework:** [nuget.org/packages/DataArc.OrchestrationFramework](https://www.nuget.org/packages/DataArc.OrchestrationFramework)
- **NuGet — DataArc.EntityFrameworkCore:** [nuget.org/packages/DataArc.EntityFrameworkCore](https://www.nuget.org/packages/DataArc.EntityFrameworkCore)
- **NuGet — DataArc.EntityFrameworkCore.SqlServer:** [nuget.org/packages/DataArc.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/DataArc.EntityFrameworkCore.SqlServer)

---

## The idea in one sentence

> **Multiple `DbContext` models can remain application boundaries without becoming relational boundaries, while DataArc keeps the application workflow explicit and .NET Aspire keeps the applications observable.**

Built by **Solid Arc Software**.
