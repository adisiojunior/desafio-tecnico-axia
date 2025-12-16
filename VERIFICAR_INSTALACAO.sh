#!/bin/bash
# Script de Verificação - Desafio Técnico Axia
# Verifica se tudo está funcionando corretamente após o clone

echo "🔍 Verificando instalação do projeto..."
echo ""

erros=0
sucessos=0

# 1. Verificar .NET SDK
echo "1️⃣ Verificando .NET 8 SDK..."
if command -v dotnet &> /dev/null; then
    dotnet_version=$(dotnet --version)
    if [[ $dotnet_version == 8.* ]]; then
        echo "   ✅ .NET SDK $dotnet_version encontrado"
        ((sucessos++))
    else
        echo "   ❌ .NET 8 SDK não encontrado. Versão atual: $dotnet_version"
        ((erros++))
    fi
else
    echo "   ❌ .NET SDK não está instalado ou não está no PATH"
    ((erros++))
fi

# 2. Verificar Docker
echo "2️⃣ Verificando Docker..."
if command -v docker &> /dev/null; then
    docker_version=$(docker --version)
    echo "   ✅ Docker encontrado: $docker_version"
    ((sucessos++))
else
    echo "   ❌ Docker não está instalado ou não está no PATH"
    ((erros++))
fi

# 3. Verificar Docker Compose
echo "3️⃣ Verificando Docker Compose..."
if command -v docker-compose &> /dev/null || docker compose version &> /dev/null; then
    compose_version=$(docker compose version 2>/dev/null || docker-compose --version)
    echo "   ✅ Docker Compose encontrado: $compose_version"
    ((sucessos++))
else
    echo "   ❌ Docker Compose não está instalado"
    ((erros++))
fi

# 4. Verificar arquivos essenciais
echo "4️⃣ Verificando arquivos do projeto..."
arquivos_essenciais=(
    "DesafioTecnicoAxia.sln"
    "docker-compose.yml"
    "Dockerfile"
    "README.md"
    "DesafioTecnicoAxia.WebApi/DesafioTecnicoAxia.WebApi.csproj"
    "DesafioTecnicoAxia.Application/DesafioTecnicoAxia.Application.csproj"
    "DesafioTecnicoAxia.Domain/DesafioTecnicoAxia.Domain.csproj"
    "DesafioTecnicoAxia.Infra/DesafioTecnicoAxia.Infra.csproj"
)

todos_arquivos_ok=true
for arquivo in "${arquivos_essenciais[@]}"; do
    if [ -f "$arquivo" ]; then
        echo "   ✅ $arquivo"
    else
        echo "   ❌ $arquivo não encontrado"
        todos_arquivos_ok=false
        ((erros++))
    fi
done

if [ "$todos_arquivos_ok" = true ]; then
    ((sucessos++))
fi

# 5. Verificar se o projeto compila
echo "5️⃣ Verificando se o projeto compila..."
if dotnet build --no-restore > /dev/null 2>&1; then
    echo "   ✅ Projeto compila com sucesso"
    ((sucessos++))
else
    echo "   ❌ Erro ao compilar o projeto"
    echo "   Execute 'dotnet build' para ver os detalhes"
    ((erros++))
fi

# 6. Verificar se os testes passam
echo "6️⃣ Verificando testes..."
if dotnet test --no-build --verbosity quiet > /dev/null 2>&1; then
    echo "   ✅ Todos os testes passaram"
    ((sucessos++))
else
    echo "   ⚠️ Alguns testes falharam (pode ser normal se o banco não estiver rodando)"
fi

# Resumo
echo ""
echo "📊 Resumo da Verificação:"
echo "   ✅ Sucessos: $sucessos"
echo "   ❌ Erros: $erros"
echo ""

if [ $erros -eq 0 ]; then
    echo "🎉 Tudo pronto! Você pode executar:"
    echo "   docker-compose up --build"
else
    echo "⚠️ Corrija os erros acima antes de executar o projeto"
fi

echo ""

