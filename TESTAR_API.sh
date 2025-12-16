#!/bin/bash
# Script de Teste da API - Desafio Técnico Axia
# Testa se a API está funcionando corretamente após iniciar

BASE_URL="${1:-http://localhost:8080}"
TIMEOUT=30

echo "🧪 Testando API de Veículos..."
echo "   Base URL: $BASE_URL"
echo ""

erros=0
sucessos=0

# Função para fazer requisições HTTP
test_endpoint() {
    local method=$1
    local url=$2
    local body=$3
    local description=$4
    
    if [ -n "$body" ]; then
        response=$(curl -s -w "\n%{http_code}" -X "$method" "$url" \
            -H "Content-Type: application/json" \
            -d "$body" \
            --max-time $TIMEOUT 2>&1)
    else
        response=$(curl -s -w "\n%{http_code}" -X "$method" "$url" \
            -H "Content-Type: application/json" \
            --max-time $TIMEOUT 2>&1)
    fi
    
    http_code=$(echo "$response" | tail -n1)
    body_response=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo "   ✅ $description"
        return 0
    else
        echo "   ❌ $description (Status: $http_code)"
        return 1
    fi
}

# 1. Health Check
echo "1️⃣ Testando Health Check..."
if test_endpoint "GET" "$BASE_URL/health" "" "Health Check"; then
    ((sucessos++))
else
    ((erros++))
fi

# 2. Swagger
echo "2️⃣ Verificando Swagger..."
http_code=$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/swagger" --max-time $TIMEOUT)
if [ "$http_code" -eq 200 ]; then
    echo "   ✅ Swagger UI acessível"
    ((sucessos++))
else
    echo "   ❌ Swagger UI não acessível (Status: $http_code)"
    ((erros++))
fi

# 3. Listar veículos (GET)
echo "3️⃣ Testando GET /api/veiculo (listar)..."
if test_endpoint "GET" "$BASE_URL/api/veiculo?page=1&pageSize=10" "" "Listar veículos"; then
    ((sucessos++))
else
    ((erros++))
fi

# 4. Criar veículo (POST)
echo "4️⃣ Testando POST /api/veiculo (criar)..."
novo_veiculo='{
  "descricao": "Carro de teste",
  "marca": 1,
  "modelo": "Teste Modelo",
  "valor": 100000.00
}'

if test_endpoint "POST" "$BASE_URL/api/veiculo" "$novo_veiculo" "Criar veículo"; then
    ((sucessos++))
    
    # Tentar obter o ID do veículo criado
    veiculos_response=$(curl -s "$BASE_URL/api/veiculo?page=1&pageSize=1" --max-time $TIMEOUT)
    veiculo_id=$(echo "$veiculos_response" | grep -o '"id":"[^"]*"' | head -1 | cut -d'"' -f4)
    
    if [ -n "$veiculo_id" ]; then
        # 5. Obter veículo por ID (GET)
        echo "5️⃣ Testando GET /api/veiculo/{id}..."
        if test_endpoint "GET" "$BASE_URL/api/veiculo/$veiculo_id" "" "Obter veículo por ID"; then
            ((sucessos++))
        else
            ((erros++))
        fi
        
        # 6. Atualizar veículo (PUT)
        echo "6️⃣ Testando PUT /api/veiculo/{id}..."
        veiculo_atualizado="{
          \"id\": \"$veiculo_id\",
          \"descricao\": \"Carro atualizado\",
          \"marca\": 1,
          \"modelo\": \"Teste Modelo Atualizado\",
          \"valor\": 120000.00
        }"
        
        if test_endpoint "PUT" "$BASE_URL/api/veiculo/$veiculo_id" "$veiculo_atualizado" "Atualizar veículo"; then
            ((sucessos++))
        else
            ((erros++))
        fi
        
        # 7. Deletar veículo (DELETE)
        echo "7️⃣ Testando DELETE /api/veiculo/{id}..."
        http_code=$(curl -s -o /dev/null -w "%{http_code}" -X DELETE "$BASE_URL/api/veiculo/$veiculo_id" --max-time $TIMEOUT)
        if [ "$http_code" -eq 204 ]; then
            echo "   ✅ Deletar veículo"
            ((sucessos++))
        else
            echo "   ❌ Deletar veículo (Status: $http_code)"
            ((erros++))
        fi
    fi
else
    ((erros++))
fi

# Resumo
echo ""
echo "📊 Resumo dos Testes:"
echo "   ✅ Sucessos: $sucessos"
echo "   ❌ Erros: $erros"
echo ""

if [ $erros -eq 0 ]; then
    echo "🎉 Todos os testes passaram! A API está funcionando corretamente."
    echo ""
    echo "📚 Acesse o Swagger para mais testes:"
    echo "   $BASE_URL/swagger"
else
    echo "⚠️ Alguns testes falharam. Verifique os logs da API."
fi

echo ""

