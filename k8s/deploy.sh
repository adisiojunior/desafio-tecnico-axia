#!/bin/bash

# Script de deploy para Kubernetes
# Uso: ./deploy.sh

echo "🚀 Iniciando deploy no Kubernetes..."

# Aplicar namespace
kubectl apply -f namespace.yaml

# Aplicar secrets
kubectl apply -f secret.yaml

# Aplicar configmap
kubectl apply -f configmap.yaml

# Aplicar PostgreSQL
kubectl apply -f postgres-deployment.yaml

# Aguardar PostgreSQL estar pronto
echo "⏳ Aguardando PostgreSQL estar pronto..."
kubectl wait --for=condition=ready pod -l app=postgres -n desafio-axia --timeout=120s

# Aplicar API
kubectl apply -f api-deployment.yaml

# Aplicar Ingress (opcional)
kubectl apply -f ingress.yaml

echo "✅ Deploy concluído!"
echo "📊 Verificando status dos pods..."
kubectl get pods -n desafio-axia

