# Infrastructure

**EF Core**: `AppDbContext` + mapeamentos + migrations.

### SQLite (dev)
- Connection string default: `Data Source=vaccination.db` (em `src/Api/appsettings.json`).
- Criar/atualizar DB:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add <Nome> -p src/Infrastructure -s src/Api
dotnet ef database update -p src/Infrastructure -s src/Api
````

### Trocar de banco

* PostgreSQL: `UseNpgsql("<connstring>")`
* SQL Server: `UseSqlServer("<connstring>")`

Ajuste em `Infrastructure/DependencyInjection.cs`.
