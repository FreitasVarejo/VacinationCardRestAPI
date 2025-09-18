# Diagrama de Classes – Cartão de Vacinação

```mermaid
classDiagram
    class Pessoa {
        +Guid Id
        +string Nome
        --
        +CartaoVacinacao Cartao
        +CriarCartao(): void
    }

    class CartaoVacinacao {
        +Guid Id
        +Guid PessoaId
        +IReadOnlyList~RegistroVacinacao~ Registros
        --
        +AdicionarRegistro(RegistroVacinacao r): void
        +RemoverRegistro(Guid registroId): void
    }

    class Vacina {
        +Guid Id
        +string Nome
        +IReadOnlyList~int~ DosesPermitidas
    }

    class RegistroVacinacao {
        +Guid Id
        +Guid VacinaId
        +DateOnly DataAplicacao
        +int Dose
    }

    Pessoa "1" --> "1" CartaoVacinacao
    CartaoVacinacao "1" o-- "many" RegistroVacinacao
    RegistroVacinacao --> Vacina
