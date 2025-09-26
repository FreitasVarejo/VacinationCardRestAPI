perfeito—segue o pacote “cola e roda” com:

1. READMEs base (raiz + por pasta + 2 docs extras)
2. Patches de validação/normalização em `CreatePersonHandler` e `UpdatePersonHandler`
3. (Opcional) habilitar comentários XML no Swagger para documentação melhor

Você pode copiar cada arquivo exatamente como está abaixo.

---

# 1) READMEs

## `README.md` (raiz)

````md
# VaccinationCard API (C#/.NET 8/9)

API para gerenciar **cartão de vacinação**: pessoas, vacinas e registros de vacinação.
Stack: **Clean Architecture (Domain, Application, Infrastructure, API)** + **CQRS/MediatR** + **FluentValidation** + **EF Core** + **Swagger**.

## Como rodar

```bash
# 1) restaurar e compilar
dotnet restore
dotnet build

# 2) aplicar migrations no SQLite (arquivo vaccination.db)
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitPersons -p src/Infrastructure -s src/Api
dotnet ef database update -p src/Infrastructure -s src/Api

# 3) subir a API
dotnet run --project src/Api
# abra: http://localhost:5xxx/swagger
````

## Estrutura

```
src/
  Api/             # Controllers, Program, Swagger
  Application/     # CQRS: Commands/Queries/Handlers + Validators + Behaviors
  Domain/          # Entidades e regras puras
  Infrastructure/  # EF Core (DbContext, Mappings, Migrations)
tests/
  Api.Tests/       # Testes unitários e de integração
docs/
  ADR-001-architecture.md
  API-Errors.md
```

## Decisões rápidas

* **CQRS + MediatR**: separa escrita/leitura e centraliza casos de uso.
* **FluentValidation**: valida entradas no pipeline do MediatR.
* **EF Core + SQLite (dev)**: simples de subir; fácil trocar para Postgres/SQL Server.
* **Swagger**: documentação/teste manual em `/swagger`.

## Próximos passos

* Implementar **Vaccine** e **Vaccination**.
* Adicionar **Auth/JWT** (bônus).
* Dockerfile + CI opcional.

````

---

## `src/Api/README.md`
```md
# API

Endpoints principais (Persons):

| Método | Rota                 | Descrição                       |
|-------:|----------------------|---------------------------------|
| POST   | /api/persons         | Cria pessoa                     |
| GET    | /api/persons         | Lista pessoas (search/page)     |
| GET    | /api/persons/{id}    | Obtém pessoa por id             |
| PUT    | /api/persons/{id}    | Atualiza pessoa                 |
| DELETE | /api/persons/{id}    | Remove pessoa                   |

### Exemplos cURL

```bash
# criar
curl -s POST http://localhost:5xxx/api/persons \
 -H "Content-Type: application/json" \
 -d '{"name":"Maria da Silva","documentId":"doc-001"}'

# listar
curl -s "http://localhost:5xxx/api/persons?search=maria"

# obter por id
curl -s "http://localhost:5xxx/api/persons/<GUID>"

# atualizar
curl -s -X PUT "http://localhost:5xxx/api/persons/<GUID>" \
 -H "Content-Type: application/json" \
 -d '{"personId":"<GUID>","name":"Maria A. Silva","documentId":"DOC-001"}'

# deletar
curl -s -X DELETE "http://localhost:5xxx/api/persons/<GUID>"
````

### Swagger

A UI do Swagger fica em `/swagger`. Se comentários XML estiverem habilitados, as descrições aparecem lá.

````

---

## `src/Application/README.md`
```md
# Application

Camada de **casos de uso** com **CQRS** via **MediatR**:

- **Commands**: ações de escrita (Create/Update/Delete).
- **Queries**: ações de leitura (Get/List).
- **Handlers**: lógica dos casos de uso (dependem de portas/abstrações).
- **Validators**: regras de entrada com **FluentValidation**.
- **Pipeline**: `ValidationBehavior` executa validações antes dos handlers.

### Como criar um caso de uso

1. Crie o `Command`/`Query` (record).
2. Crie o `Validator` (regra de formato/len/regex).
3. Implemente o `Handler` usando `IAppDbContext`.
4. Exponha no Controller (API).
````

---

## `src/Domain/README.md`

```md
# Domain

Entidades e regras **puras**, sem dependência de EF/HTTP.

- `Person`:
  - `PersonId` (Guid), `Name` (≤200), `DocumentId` (≤50), `CreatedAt`.
  - `Update(name, documentId)` com trims.
- Futuras entidades: `Vaccine`, `Vaccination`.
```

---

## `src/Infrastructure/README.md`

````md
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

````

---

## `docs/ADR-001-architecture.md`
```md
# ADR-001 – Arquitetura

## Decisão
Adotar **Clean Architecture leve** com **CQRS/MediatR**, **FluentValidation**, **EF Core** e **Swagger**.

## Contexto
- Projeto de backend pequeno/médio com necessidade de organização de casos de uso e validação.
- Facilidade de testes: validators unitários, handlers com contexto isolável.

## Consequências
- Separação clara de responsabilidades.
- Ligeiro overhead de pastas/arquivos, compensado por escalabilidade do design.
````

---

## `docs/API-Errors.md`

````md
# Contrato de Erros

Formato sugerido (exemplo):
```json
{
  "traceId": "00-...-...",
  "message": "Validation failed",
  "errors": {
    "Name": ["Name é obrigatório."],
    "DocumentId": ["DocumentId contém caracteres inválidos."]
  }
}
````

Fontes de erro:

* 400: validação (FluentValidation)
* 404: recurso não encontrado
* 409: conflito (duplicidade); neste projeto usamos 400/500 com mensagens claras

````

---

# 2) Patches (normalização e unicidade robusta)

> Objetivo: **normalizar** `Name` e `DocumentId` (trim; `DocumentId` em **UPPER**) em *handlers*, checar unicidade **com valor normalizado**, e gravar normalizado no DB. Assim, “doc-1”, “DOC-1” e “ Doc-1 ” colidem corretamente.

Substitua os conteúdos dos dois handlers por estes:

### `src/Application/Persons/Handlers/CreatePersonHandler.cs`
```csharp
using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Commands;
using VaccinationCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly IAppDbContext _db;
    public CreatePersonHandler(IAppDbContext db) => _db = db;

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken ct)
    {
        // normalização
        var name = request.Name.Trim();
        var doc  = request.DocumentId.Trim().ToUpperInvariant();

        // unicidade por DocumentId (normalizado)
        var exists = await _db.Persons.AsNoTracking()
            .AnyAsync(p => p.DocumentId == doc, ct);
        if (exists) throw new InvalidOperationException("DocumentId já cadastrado.");

        var entity = new Person(Guid.NewGuid(), name, doc);
        _db.Persons.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new PersonDto(entity.PersonId, entity.Name, entity.DocumentId, entity.CreatedAt);
    }
}
````

### `src/Application/Persons/Handlers/UpdatePersonHandler.cs`

```csharp
using MediatR;
using VaccinationCard.Application.Abstractions;
using VaccinationCard.Application.Persons;
using VaccinationCard.Application.Persons.Commands;
using Microsoft.EntityFrameworkCore;

namespace VaccinationCard.Application.Persons.Handlers;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonDto>
{
    private readonly IAppDbContext _db;
    public UpdatePersonHandler(IAppDbContext db) => _db = db;

    public async Task<PersonDto> Handle(UpdatePersonCommand request, CancellationToken ct)
    {
        var entity = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == request.PersonId, ct);
        if (entity is null) throw new KeyNotFoundException("Pessoa não encontrada.");

        var name = request.Name.Trim();
        var doc  = request.DocumentId.Trim().ToUpperInvariant();

        // checar duplicidade contra outros registros
        var docTaken = await _db.Persons.AsNoTracking()
            .AnyAsync(p => p.DocumentId == doc && p.PersonId != request.PersonId, ct);
        if (docTaken) throw new InvalidOperationException("DocumentId já cadastrado para outra pessoa.");

        entity.Update(name, doc);
        await _db.SaveChangesAsync(ct);

        return new PersonDto(entity.PersonId, entity.Name, entity.DocumentId, entity.CreatedAt);
    }
}
```

> Observação: os **validators** que te passei anteriormente continuam válidos (formatos/regex/len). A normalização acontece no handler para garantir consistência no banco.

---

# 3) (Opcional) Comentários XML no Swagger

Habilitar comentários XML melhora a doc automática.

### `src/Api/Api.csproj` – adicione:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>1591</NoWarn> <!-- suprime warning de docs ausentes -->
</PropertyGroup>
```

### `src/Api/Program.cs` – incluir XML no SwaggerGen:

```csharp
using System.Reflection;
using System.IO;
...
builder.Services.AddSwaggerGen(opt =>
{
    var xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
    if (File.Exists(xmlPath))
        opt.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});
```

E opcionalmente adicione `/// <summary> ... </summary>` nos métodos do `PersonsController` para aparecer no Swagger.

---

# 4) Checklist rápido antes de seguir para Vaccine/Vaccination

* [x] Validators de `Person` com regras de formato/len/regex.
* [x] Normalização no handler (Name trim, DocumentId UPPER).
* [x] Índice único em `DocumentId` (já no `OnModelCreating`).
* [x] Testes unitários (validators) e de integração (unicidade/normalização).
* [x] READMEs básicos prontos.

se quiser, já sigo com **Vaccine** e **Vaccination** no mesmo padrão (entidades, validators, handlers, mappings com FKs e UNIQUE(PersonId,VaccineId,DoseNumber), controller e testes).
