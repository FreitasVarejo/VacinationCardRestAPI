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
