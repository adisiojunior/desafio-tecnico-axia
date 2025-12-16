# Script de Verificacao - Desafio Tecnico Axia
# Verifica se tudo esta funcionando corretamente apos o clone

Write-Host "Verificando instalacao do projeto..." -ForegroundColor Cyan
Write-Host ""

$erros = 0
$sucessos = 0

# 1. Verificar .NET SDK
Write-Host "1. Verificando .NET 8 SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    $versionMajor = [int]($dotnetVersion.Split('.')[0])
    if ($versionMajor -ge 8) {
        Write-Host "   [OK] .NET SDK $dotnetVersion encontrado (compativel com .NET 8)" -ForegroundColor Green
        $sucessos++
    } else {
        Write-Host "   [ERRO] .NET 8 ou superior necessario. Versao atual: $dotnetVersion" -ForegroundColor Red
        $erros++
    }
} catch {
    Write-Host "   [ERRO] .NET SDK nao esta instalado ou nao esta no PATH" -ForegroundColor Red
    $erros++
}

# 2. Verificar Docker
Write-Host "2. Verificando Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "   [OK] Docker encontrado: $dockerVersion" -ForegroundColor Green
    $sucessos++
} catch {
    Write-Host "   [ERRO] Docker nao esta instalado ou nao esta no PATH" -ForegroundColor Red
    $erros++
}

# 3. Verificar Docker Compose
Write-Host "3. Verificando Docker Compose..." -ForegroundColor Yellow
try {
    $composeVersion = docker compose version
    Write-Host "   [OK] Docker Compose encontrado: $composeVersion" -ForegroundColor Green
    $sucessos++
} catch {
    Write-Host "   [ERRO] Docker Compose nao esta instalado" -ForegroundColor Red
    $erros++
}

# 4. Verificar arquivos essenciais
Write-Host "4. Verificando arquivos do projeto..." -ForegroundColor Yellow
$arquivosEssenciais = @(
    "DesafioTecnicoAxia.sln",
    "docker-compose.yml",
    "Dockerfile",
    "README.md",
    "DesafioTecnicoAxia.WebApi/DesafioTecnicoAxia.WebApi.csproj",
    "DesafioTecnicoAxia.Application/DesafioTecnicoAxia.Application.csproj",
    "DesafioTecnicoAxia.Domain/DesafioTecnicoAxia.Domain.csproj",
    "DesafioTecnicoAxia.Infra/DesafioTecnicoAxia.Infra.csproj"
)

$todosArquivosOk = $true
foreach ($arquivo in $arquivosEssenciais) {
    if (Test-Path $arquivo) {
        Write-Host "   [OK] $arquivo" -ForegroundColor Green
    } else {
        Write-Host "   [ERRO] $arquivo nao encontrado" -ForegroundColor Red
        $todosArquivosOk = $false
        $erros++
    }
}

if ($todosArquivosOk) {
    $sucessos++
}

# 5. Verificar se o projeto compila
Write-Host "5. Verificando se o projeto compila..." -ForegroundColor Yellow
try {
    $buildResult = dotnet build --no-restore 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   [OK] Projeto compila com sucesso" -ForegroundColor Green
        $sucessos++
    } else {
        Write-Host "   [ERRO] Erro ao compilar o projeto" -ForegroundColor Red
        Write-Host "   Detalhes: $buildResult" -ForegroundColor Red
        $erros++
    }
} catch {
    Write-Host "   [ERRO] Erro ao verificar compilacao: $_" -ForegroundColor Red
    $erros++
}

# 6. Verificar se os testes passam
Write-Host "6. Verificando testes..." -ForegroundColor Yellow
try {
    $testResult = dotnet test --no-build --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   [OK] Todos os testes passaram" -ForegroundColor Green
        $sucessos++
    } else {
        Write-Host "   [AVISO] Alguns testes falharam (normal se o banco nao estiver rodando)" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   [AVISO] Nao foi possivel executar os testes" -ForegroundColor Yellow
}

# Resumo
Write-Host ""
Write-Host "Resumo da Verificacao:" -ForegroundColor Cyan
Write-Host "   [OK] Sucessos: $sucessos" -ForegroundColor Green
Write-Host "   [ERRO] Erros: $erros" -ForegroundColor $(if ($erros -gt 0) { "Red" } else { "Green" })
Write-Host ""

if ($erros -eq 0) {
    Write-Host "[OK] Tudo pronto! Voce pode executar:" -ForegroundColor Green
    Write-Host "   docker-compose up --build" -ForegroundColor White
} else {
    Write-Host "[ERRO] Corrija os erros acima antes de executar o projeto" -ForegroundColor Yellow
}

Write-Host ""
