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
