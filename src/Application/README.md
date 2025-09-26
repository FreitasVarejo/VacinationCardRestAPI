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
