PROJECT_NAME=vaccination-card-api

.PHONY: build run stop logs test

# builda a imagem (inclui restore, build, test e publish no Dockerfile)
build:
	docker build -t $(PROJECT_NAME) .

# sobe a API em background
run:
	docker run -d -p 5000:8080 --name $(PROJECT_NAME) $(PROJECT_NAME)

# para e remove o container
stop:
	docker stop $(PROJECT_NAME) || true
	docker rm $(PROJECT_NAME) || true

# logs da API
logs:
	docker logs -f $(PROJECT_NAME)

# roda testes no host (não dentro do container)
test:
	dotnet test tests/Api.Tests/Api.Tests.csproj -c Release
