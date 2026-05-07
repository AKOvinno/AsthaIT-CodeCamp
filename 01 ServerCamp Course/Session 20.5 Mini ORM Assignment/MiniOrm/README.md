# MiniOrm — Scratch ORM with ADO.NET

A simplified Entity Framework–style ORM built on top of PostgreSQL using ADO.NET and Npgsql. No EF Core, no Dapper — raw SQL generated at runtime via C# reflection.

---

## Projects

| Project | Purpose |
|---|---|
| `MiniOrm/` | Core ORM library + demo `Program.cs` |
| `MiniOrm.Migrations/` | CLI migration tool |

---

## Prerequisites

- .NET 8 SDK
- PostgreSQL running locally (or remote)

---

## Setup

### 1. Create the database

```sql
CREATE DATABASE miniorm;
```

### 2. Set the connection string

```bash
# Linux / macOS
export MINIORM_CONN="Host=localhost;Database=miniorm;Username=postgres;Password=secret"

# Windows (PowerShell)
$env:MINIORM_CONN="Host=localhost;Database=miniorm;Username=postgres;Password=secret"
```

---

## Running Migrations

```bash
cd MiniOrm.Migrations

# Generate migration SQL file (does NOT touch the DB)
dotnet run -- migrations add InitialCreate

# Review the generated file in Migrations/
# Apply all pending migrations
dotnet run -- migrations apply

# List applied / pending
dotnet run -- migrations list

# Rollback the last applied migration
dotnet run -- migrations rollback
```

---

## Running the Demo

```bash
cd MiniOrm
dotnet run
```

The demo:
1. Connects to PostgreSQL
2. Inserts two `Product` rows (one with `Discount = NULL`)
3. Finds a row by id
4. Updates price and discount
5. Lists all rows
6. Deletes all rows

---

## Type Mapping

| C# Type | PostgreSQL Type | Nullability |
|---|---|---|
| `int` (PrimaryKey) | `SERIAL PRIMARY KEY` | — |
| `int` | `INTEGER NOT NULL` | required |
| `int?` | `INTEGER NULL` | nullable |
| `long` / `long?` | `BIGINT` | same pattern |
| `float` / `float?` | `REAL` | |
| `double` / `double?` | `DOUBLE PRECISION` | |
| `decimal` / `decimal?` | `NUMERIC` | |
| `bool` / `bool?` | `BOOLEAN` | |
| `DateTime` / `DateTime?` | `TIMESTAMP` | |
| `Guid` / `Guid?` | `UUID` | |
| `string` | `TEXT NOT NULL` | |
| `string?` | `TEXT NULL` | |

Nullable value types are detected with `Nullable.GetUnderlyingType()`.  
Nullable reference types (`string?`) are detected via `NullabilityInfoContext`.

---

## Attribute Filtering

Only properties decorated with `[Column]` or `[PrimaryKey]` are mapped. Navigation properties and any unmapped properties are silently ignored — this mirrors EF Core's explicit-column convention.

---

## Architecture

```
MiniOrm/
├── Attributes/
│   ├── TableAttribute.cs       — [Table("table_name")]
│   ├── ColumnAttribute.cs      — [Column("col_name")]
│   └── PrimaryKeyAttribute.cs  — [PrimaryKey]
├── Data/
│   ├── DbContext.cs            — Base context; auto-initialises DbSet<T> properties
│   ├── DbSet.cs                — Generic CRUD (Insert/FindById/GetAll/Update/Delete)
│   ├── TypeMapper.cs           — C# type → PostgreSQL type string
│   └── EntityMetadata.cs       — Cached reflection metadata for an entity
├── Models/
│   ├── Product.cs
│   └── Order.cs
└── Program.cs                  — Step-by-step demo

MiniOrm.Migrations/
├── Commands/
│   └── MigrationRunner.cs      — add / apply / list / rollback logic
└── Program.cs                  — CLI entry point
```
