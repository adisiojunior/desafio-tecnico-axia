# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["DesafioTecnicoAxia.WebApi/DesafioTecnicoAxia.WebApi.csproj", "DesafioTecnicoAxia.WebApi/"]
COPY ["DesafioTecnicoAxia.Application/DesafioTecnicoAxia.Application.csproj", "DesafioTecnicoAxia.Application/"]
COPY ["DesafioTecnicoAxia.Domain/DesafioTecnicoAxia.Domain.csproj", "DesafioTecnicoAxia.Domain/"]
COPY ["DesafioTecnicoAxia.Infra/DesafioTecnicoAxia.Infra.csproj", "DesafioTecnicoAxia.Infra/"]

RUN dotnet restore "DesafioTecnicoAxia.WebApi/DesafioTecnicoAxia.WebApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DesafioTecnicoAxia.WebApi"
RUN dotnet build "DesafioTecnicoAxia.WebApi.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "DesafioTecnicoAxia.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "DesafioTecnicoAxia.WebApi.dll"]

