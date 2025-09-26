# Domain

Entidades e regras **puras**, sem dependência de EF/HTTP.

- `Person`:
  - `PersonId` (Guid), `Name` (≤200), `DocumentId` (≤50), `CreatedAt`.
  - `Update(name, documentId)` com trims.
- Futuras entidades: `Vaccine`, `Vaccination`.
