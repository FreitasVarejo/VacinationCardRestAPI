# 🌱 Branch-by-feature — Plano de projetos

Cada item abaixo é **um projeto pequeno** com escopo fechado. Sugiro este fluxo: crie a branch, implemente, abra PR, revise, merge. Mantém você focado e dá rastreabilidade — perfeito pra estágio.

## 1) `feat/auth-jwt` – Autenticação e Autorização

**Objetivo:** proteger endpoints com **JWT** (bearer).
**Tarefas:**

* Adicionar `Microsoft.AspNetCore.Authentication.JwtBearer`
* Configurar `Program.cs` (JWT bearer, `AddAuthorization`)
* Criar endpoint de login fake (retorna token assinado com secret local)
* Marcar endpoints com `[Authorize]`
  **Aceite:** acessar endpoint sem token → 401; com token válido → 200.
  **Testes:** um teste de integração mínimo validando 401/200.

## 2) `feat/serilog-logging` – Observabilidade (logs)

**Objetivo:** logs estruturados com **Serilog** + request logging.
**Tarefas:**

* Add `Serilog.AspNetCore`, `Serilog.Sinks.Console`
* Middleware de correlação (`X-Correlation-Id`)
* Enriquecedores: requestId, userId (se autenticado), rota
  **Aceite:** logs JSON no console com correlation id.

## 3) `feat/efcore-optimizations` – EF Core avançado

**Objetivo:** otimizar o acesso a dados.
**Tarefas:**

* `AsNoTracking` para queries read-only
* Interceptor de comandos (log de SQL lento)
* Índices (ex.: `Vaccine.Code`), `Include` seletivo, paginação
  **Aceite:** endpoints de listagem com paginação + logging de queries lentas.

## 4) `feat/dapper-readmodel` – Read Model performático

**Objetivo:** CQRS na prática: **leitura** com **Dapper**, **escrita** com EF.
**Tarefas:**

* Adicionar Dapper (apenas na camada de leitura)
* Criar query handler que usa Dapper para `/people/{id}/card`
  **Aceite:** resposta idêntica ao EF, porém usando Dapper.

## 5) `feat/grpc-internal` – Comunicação entre serviços

**Objetivo:** expor **gRPC** para operações internas (ex.: `GetVaccineByCode`).
**Tarefas:**

* Add pacote Grpc.AspNetCore
* `.proto` + service simples
* Gate por `Authorization` (JWT)
  **Aceite:** client local chama método e recebe dados.

## 6) `feat/signalr-realtime` – Tempo real

**Objetivo:** notificar UI quando novo registro for criado.
**Tarefas:**

* Add **SignalR**, hub `/hubs/notifications`
* Ao criar `VaccinationEntry`, enviar evento para o hub
  **Aceite:** client JS recebe mensagem “entry-created”.

## 7) `feat/messaging-masstransit-rabbitmq` – Mensageria

**Objetivo:** publicar eventos de domínio (ex.: `VaccinationRecorded`) via **MassTransit** + **RabbitMQ**.
**Tarefas:**

* Subir RabbitMQ (docker compose override)
* Producer no handler de `RecordVaccination`
* Consumer logando evento
  **Aceite:** evento publicado e consumido (logs mostram payload).

## 8) `feat/opentelemetry-tracing` – Tracing distribuído

**Objetivo:** coletar traces com **OpenTelemetry** (HTTP, EF, gRPC, MassTransit).
**Tarefas:**

* Add `OpenTelemetry.Instrumentation.*` e exporter OTLP
* Docker compose com `otel-collector` + `Jaeger` ou `Grafana Tempo`
  **Aceite:** ver trace de uma request no Jaeger (com spans de DB/HTTP).

## 9) `feat/azure-deploy` – Deploy Cloud

**Objetivo:** preparar deploy para Azure App Service.
**Tarefas:**

* Dockerfile final + GitHub Actions para push no GHCR
* Pipeline de deploy (manual) para Azure Web App
  **Aceite:** app rodando em URL pública com Swagger.

## 10) `feat/tests-hardening` – Qualidade

**Objetivo:** reforçar testes e coverage.
**Tarefas:**

* xUnit para domain services e handlers
* Testes de integração com `WebApplicationFactory`
* Configurar `coverlet` + report no CI
  **Aceite:** build do CI exibe cobertura > 60% (ajuste a meta).

---

# 🧭 Convenções e comandos úteis

## Ramo & PR

```bash
git checkout -b feat/auth-jwt
# ... coding ...
git commit -m "feat(auth): add JWT bearer auth"
git push -u origin feat/auth-jwt
# Abra PR para main (use o template)
```

## Merges limpos

* Rebase antes de abrir o PR: `git fetch origin && git rebase origin/main`
* Resolve conflitos localmente; mantenha commits pequenos.
* PR < 300–500 linhas. Se passar disso, quebre.

# 🧱 Sequência sugerida (4–8 semanas)

1. Repo Zero + `feat/auth-jwt`
2. `feat/serilog-logging`
3. `feat/efcore-optimizations`
4. `feat/dapper-readmodel`
5. `feat/signalr-realtime`
6. `feat/messaging-masstransit-rabbitmq`
7. `feat/opentelemetry-tracing`
8. `feat/azure-deploy`
9. `feat/tests-hardening`

