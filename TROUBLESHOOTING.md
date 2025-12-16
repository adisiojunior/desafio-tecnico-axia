# 🔧 Troubleshooting - Resolução de Problemas

## Erro 500 ao Criar Veículo

### 1. Verificar Logs Detalhados

Com as melhorias implementadas, os logs agora mostram mais detalhes em desenvolvimento. Verifique os logs do container:

```bash
docker-compose logs -f api
```

### 2. Verificar Conexão com PostgreSQL

```bash
# Verificar se o PostgreSQL está rodando
docker-compose ps postgres

# Verificar logs do PostgreSQL
docker-compose logs postgres

# Testar conexão
docker-compose exec postgres psql -U postgres -d VeiculosDb -c "SELECT 1;"
```

### 3. Aplicar Migrations Manualmente

Se as migrations não foram aplicadas automaticamente:

```bash
# Entrar no container da API
docker-compose exec api bash

# Aplicar migrations
dotnet ef database update --project /src/DesafioTecnicoAxia.Infra --startup-project /src/DesafioTecnicoAxia.WebApi
```

Ou fora do container:

```bash
# Se estiver rodando localmente
dotnet ef database update --project DesafioTecnicoAxia.Infra --startup-project DesafioTecnicoAxia.WebApi
```

### 4. Verificar String de Conexão

A string de conexão deve estar correta no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=VeiculosDb;Username=postgres;Password=postgres"
  }
}
```

### 5. Reconstruir Containers

Se nada funcionar, tente reconstruir:

```bash
docker-compose down -v
docker-compose up --build
```

### 6. Verificar Health Check

```bash
curl http://localhost:8080/health
```

Se retornar erro, o problema está na conexão com o banco.

## Erro de Rota (404)

### Verificar URL Correta

A rota correta é:
- ✅ `http://localhost:8080/api/veiculo` (minúsculo)
- ❌ `http://localhost:8080/api/Veiculo` (maiúsculo)

## Erro de Validação (400)

### Verificar Dados Enviados

Certifique-se de que está enviando:
- `descricao` (obrigatório, string)
- `marca` (obrigatório, número de 1 a 10)
- `modelo` (obrigatório, string)
- `opcionais` (opcional, string)
- `valor` (opcional, número > 0)

Exemplo correto:
```json
{
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "valor": 250000.00
}
```

## Verificar Status dos Serviços

```bash
# Ver status de todos os containers
docker-compose ps

# Ver logs em tempo real
docker-compose logs -f

# Ver logs apenas da API
docker-compose logs -f api

# Ver logs apenas do PostgreSQL
docker-compose logs -f postgres
```

## Resetar Tudo

Se quiser começar do zero:

```bash
# Parar e remover tudo
docker-compose down -v

# Reconstruir e iniciar
docker-compose up --build
```

## Verificar Portas

Se a porta 8080 estiver em uso:

1. Altere no `docker-compose.yml`:
```yaml
ports:
  - "8081:8080"  # Use 8081 ao invés de 8080
```

2. Acesse em `http://localhost:8081/swagger`

