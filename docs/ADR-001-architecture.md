# ADR-001 – Arquitetura

## Decisão
Adotar **Clean Architecture leve** com **CQRS/MediatR**, **FluentValidation**, **EF Core** e **Swagger**.

## Contexto
- Projeto de backend pequeno/médio com necessidade de organização de casos de uso e validação.
- Facilidade de testes: validators unitários, handlers com contexto isolável.

## Consequências
- Separação clara de responsabilidades.
- Ligeiro overhead de pastas/arquivos, compensado por escalabilidade do design.
