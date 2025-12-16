# 🚀 Guia de Deploy - Desafio Técnico Axia

Este guia explica como fazer o deploy da aplicação usando Docker e Kubernetes.

## 📦 Docker

### Desenvolvimento Local

1. **Construir e executar com docker-compose:**
```bash
docker-compose up --build
```

2. **Executar em background:**
```bash
docker-compose up -d
```

3. **Ver logs:**
```bash
docker-compose logs -f api
```

4. **Parar os containers:**
```bash
docker-compose down
```

5. **Parar e remover volumes:**
```bash
docker-compose down -v
```

### Produção

1. **Usar docker-compose de produção:**
```bash
docker-compose -f docker-compose.prod.yml up --build -d
```

2. **Definir variáveis de ambiente:**
```bash
export POSTGRES_PASSWORD=senha_segura
export POSTGRES_USER=postgres
export POSTGRES_DB=VeiculosDb

docker-compose -f docker-compose.prod.yml up -d
```

### Build Manual da Imagem

```bash
docker build -t desafio-axia-api:latest .
```

## ☸️ Kubernetes

### Pré-requisitos

- Kubernetes cluster configurado
- kubectl instalado e configurado
- Acesso ao cluster

### Deploy Completo

1. **Aplicar todos os recursos:**
```bash
cd k8s
./deploy.sh
```

Ou manualmente:

```bash
kubectl apply -f namespace.yaml
kubectl apply -f secret.yaml
kubectl apply -f configmap.yaml
kubectl apply -f postgres-deployment.yaml
kubectl apply -f api-deployment.yaml
kubectl apply -f ingress.yaml
```

2. **Verificar status:**
```bash
kubectl get pods -n desafio-axia
kubectl get services -n desafio-axia
kubectl get ingress -n desafio-axia
```

3. **Ver logs:**
```bash
kubectl logs -f deployment/api-deployment -n desafio-axia
```

### Health Checks

A API expõe os seguintes endpoints de health check:

- `/health` - Health check geral
- `/health/ready` - Verifica se está pronto para receber tráfego
- `/health/live` - Verifica se a aplicação está viva

### Escalar Aplicação

```bash
kubectl scale deployment api-deployment --replicas=5 -n desafio-axia
```

### Atualizar Imagem

1. **Buildar nova imagem:**
```bash
docker build -t desafio-axia-api:v1.1.0 .
```

2. **Atualizar deployment:**
```bash
kubectl set image deployment/api-deployment api=desafio-axia-api:v1.1.0 -n desafio-axia
```

3. **Verificar rollout:**
```bash
kubectl rollout status deployment/api-deployment -n desafio-axia
```

### Rollback

```bash
kubectl rollout undo deployment/api-deployment -n desafio-axia
```

### Remover Deploy

```bash
kubectl delete namespace desafio-axia
```

## 🔧 Configurações

### Variáveis de Ambiente

As seguintes variáveis podem ser configuradas:

- `ASPNETCORE_ENVIRONMENT` - Ambiente (Development, Production)
- `ASPNETCORE_URLS` - URLs que a API escuta
- `ConnectionStrings__DefaultConnection` - String de conexão do PostgreSQL

### Secrets

⚠️ **IMPORTANTE**: Em produção, use secrets gerenciados (Azure Key Vault, AWS Secrets Manager, etc.) ao invés de arquivos YAML.

## 📊 Monitoramento

### Verificar Health Checks

```bash
# Docker
curl http://localhost:8080/health

# Kubernetes
kubectl exec -it <pod-name> -n desafio-axia -- wget -qO- http://localhost:8080/health
```

### Métricas

A aplicação está preparada para integração com:
- Prometheus
- Grafana
- Application Insights (Azure)

## 🐛 Troubleshooting

### PostgreSQL não conecta

1. Verificar se o PostgreSQL está rodando:
```bash
# Docker
docker-compose ps postgres

# Kubernetes
kubectl get pods -l app=postgres -n desafio-axia
```

2. Verificar logs:
```bash
# Docker
docker-compose logs postgres

# Kubernetes
kubectl logs -l app=postgres -n desafio-axia
```

### API não inicia

1. Verificar logs:
```bash
# Docker
docker-compose logs api

# Kubernetes
kubectl logs -l app=api -n desafio-axia
```

2. Verificar health checks:
```bash
curl http://localhost:8080/health
```

### Migrations não aplicadas

As migrations são aplicadas automaticamente em desenvolvimento. Em produção, execute manualmente:

```bash
dotnet ef database update --project DesafioTecnicoAxia.Infra --startup-project DesafioTecnicoAxia.WebApi
```

