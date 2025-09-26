![.NET Build & Test](https://github.com/<seu-usuario>/<seu-repo>/actions/workflows/dotnet.yml/badge.svg)

# VaccinationCard API (C# / .NET 9)

API REST para gerenciar **cartão de vacinação**: cadastro de pessoas, vacinas e registros de vacinação, com arquitetura limpa, validações robustas e documentação interativa via Swagger.

## 🛠️ Stack & Decisões
- **Arquitetura:** Clean (Domain, Application, Infrastructure, API) + **CQRS (MediatR)**
- **Validação:** FluentValidation por request
- **Persistência:** EF Core com **SQLite** (dev). Fácil trocar para PostgreSQL/SQL Server
- **Documentação/Testes manuais:** Swagger/OpenAPI em `/swagger`
- **Testes:** xUnit + FluentValidation + cobertura via `dotnet test`
- **CI/CD:** GitHub Actions (`.github/workflows/dotnet.yml`)

## 📂 Estrutura de Pastas
```

src/
Api/             # Controllers, DI, Swagger
Application/     # Commands/Queries + Handlers + Validators (MediatR/FluentValidation)
Domain/          # Entidades e regras puras
Infrastructure/  # DbContext, Mappings, Migrations
tests/
Api.Tests/       # Testes (unit/integration)

````

## 🧩 Modelo de Domínio
- **Person**(PersonId, Name, DocumentId* único)
- **Vaccine**(VaccineId, Name* único, ScheduleJson?)
- **Vaccination**(VaccinationId, PersonId(FK), VaccineId(FK), DoseNumber≥1, DateApplied, Notes?)
- Regras DB: `UNIQUE(PersonId, VaccineId, DoseNumber)`; **cascade** ao excluir Person

```mermaid
classDiagram
  class Person {
    Guid PersonId
    string Name
    string DocumentId
  }
  class Vaccine {
    Guid VaccineId
    string Name
    string ScheduleJson
  }
  class Vaccination {
    Guid VaccinationId
    Guid PersonId
    Guid VaccineId
    int DoseNumber
    DateTime DateApplied
    string Notes
  }
  Person "1" --> "*" Vaccination
  Vaccine "1" --> "*" Vaccination
````

## 🚀 Como rodar localmente

```bash
# restaurar e compilar
dotnet restore
dotnet build

# aplicar migrations (SQLite → vaccination.db)
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitPersons -p src/Infrastructure -s src/Api
dotnet ef database update -p src/Infrastructure -s src/Api

# subir API
dotnet run --project src/Api
# abrir Swagger: http://localhost:5xxx/swagger
```

## 🐳 Rodando com Docker

```bash
# build da imagem
make build

# subir API (localhost:5000 → container:8080)
make run

# logs
make logs

# parar e remover container
make stop

# rodar testes localmente
make test
```

## ✅ Testes

```bash
# rodar todos os testes
dotnet test tests/Api.Tests/Api.Tests.csproj -c Release
```

CI do GitHub também executa testes automaticamente a cada push/PR.

## 📌 Roadmap

* [x] CRUD de **Person**
* [ ] CRUD de **Vaccine**
* [ ] CRUD de **Vaccination**
* [ ] Autenticação JWT
* [ ] Healthcheck no Docker
* [ ] Deploy (Docker + GHCR)

---

✍️ Autor: *Gabriel Freitas Pinheiro*
📄 Licença: MIT
