# Stage 1 - build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# garante cache de pacotes
ENV NUGET_PACKAGES=/root/.nuget/packages

# copiar csproj e solução primeiro
COPY *.sln ./
COPY src/Api/*.csproj src/Api/
COPY src/Application/*.csproj src/Application/
COPY src/Domain/*.csproj src/Domain/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY tests/Api.Tests/*.csproj tests/Api.Tests/
COPY nuget.config* ./

# copiar o resto
COPY . .

# build (já faz restore também)
RUN dotnet build VaccinationCard.sln -c Release

# rodar testes (faila build se quebrar)
RUN dotnet test tests/Api.Tests/Api.Tests.csproj -c Release --no-build --logger "trx;LogFileName=test_results.trx"

# publish só da API
RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2 - runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]
