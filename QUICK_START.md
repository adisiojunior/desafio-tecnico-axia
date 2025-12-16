# 🚀 Guia Rápido de Início

## Início Rápido com Docker (5 minutos)

### 1. Execute o Docker Compose

```bash
docker-compose up --build
```

### 2. Aguarde a mensagem "Application started"

Você verá nos logs algo como:
```
Now listening on: http://[::]:8080
Application started. Press Ctrl+C to shut down.
```

### 3. Acesse no Navegador

**🎯 PRINCIPAL - Swagger UI (Interface para testar a API):**
```
http://localhost:8080/swagger
```

Esta é a forma mais fácil de testar! O Swagger permite:
- Ver todos os endpoints disponíveis
- Testar cada endpoint diretamente no navegador
- Ver exemplos de requisições e respostas
- Executar chamadas GET, POST, PUT, DELETE

### 4. Teste os Endpoints

#### Via Swagger (Recomendado)
1. Abra `http://localhost:8080/swagger`
2. Clique em qualquer endpoint
3. Clique em "Try it out"
4. Preencha os dados (se necessário)
5. Clique em "Execute"
6. Veja a resposta

#### Via cURL ou Postman

**Listar todos os veículos:**
```bash
curl http://localhost:8080/api/veiculo
```

**Criar um veículo:**
```bash
curl -X POST http://localhost:8080/api/veiculo \
  -H "Content-Type: application/json" \
  -d '{
    "descricao": "Carro esportivo",
    "marca": 1,
    "modelo": "Mustang GT",
    "valor": 250000.00
  }'
```

**Verificar saúde da API:**
```bash
curl http://localhost:8080/health
```

## 📍 Endereços Importantes

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Swagger UI** | http://localhost:8080/swagger | Interface para testar a API |
| **API Base** | http://localhost:8080/api/veiculo | Endpoint base da API |
| **Health Check** | http://localhost:8080/health | Status da aplicação |
| **Health Ready** | http://localhost:8080/health/ready | Verifica se está pronto |
| **Health Live** | http://localhost:8080/health/live | Verifica se está vivo |

## 🔍 Verificar se está funcionando

### 1. Ver logs do container:
```bash
docker-compose logs -f api
```

### 2. Verificar health check:
```bash
curl http://localhost:8080/health
```

Deve retornar:
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.1234567",
  "entries": {
    "postgresql": {
      "status": "Healthy",
      "duration": "00:00:00.0123456"
    },
    "dbcontext": {
      "status": "Healthy",
      "duration": "00:00:00.0012345"
    }
  }
}
```

### 3. Testar endpoint:
```bash
curl http://localhost:8080/api/veiculo
```

Deve retornar uma lista (pode estar vazia inicialmente):
```json
[]
```

## 🛑 Parar a aplicação

```bash
docker-compose down
```

Para remover também os volumes (dados do banco):
```bash
docker-compose down -v
```

## ❓ Problemas Comuns

### Porta 8080 já está em uso

Altere a porta no `docker-compose.yml`:
```yaml
ports:
  - "8081:8080"  # Mude 8080 para 8081
```

### PostgreSQL não conecta

1. Verifique se o container do PostgreSQL está rodando:
```bash
docker-compose ps
```

2. Verifique os logs:
```bash
docker-compose logs postgres
```

### API não responde

1. Verifique os logs:
```bash
docker-compose logs api
```

2. Verifique o health check:
```bash
curl http://localhost:8080/health
```

## 📚 Próximos Passos

- Leia o [README.md](README.md) completo para mais detalhes
- Veja [DEPLOY.md](DEPLOY.md) para deploy em produção
- Explore o Swagger em `http://localhost:8080/swagger`

