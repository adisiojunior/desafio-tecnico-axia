# Script de Teste da API - Desafio Técnico Axia
# Testa se a API está funcionando corretamente após iniciar

param(
    [string]$BaseUrl = "http://localhost:8080",
    [int]$TimeoutSeconds = 30
)

Write-Host "🧪 Testando API de Veículos..." -ForegroundColor Cyan
Write-Host "   Base URL: $BaseUrl" -ForegroundColor Gray
Write-Host ""

$erros = 0
$sucessos = 0

# Função para fazer requisições HTTP
function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [string]$Body = $null,
        [string]$Description
    )
    
    try {
        $headers = @{
            "Content-Type" = "application/json"
        }
        
        if ($Body) {
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Headers $headers -Body $Body -TimeoutSec $TimeoutSeconds
        } else {
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Headers $headers -TimeoutSec $TimeoutSeconds
        }
        
        Write-Host "   ✅ $Description" -ForegroundColor Green
        return $true
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        Write-Host "   ❌ $Description (Status: $statusCode)" -ForegroundColor Red
        Write-Host "      Erro: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# 1. Health Check
Write-Host "1️⃣ Testando Health Check..." -ForegroundColor Yellow
if (Test-Endpoint -Method "GET" -Url "$BaseUrl/health" -Description "Health Check") {
    $sucessos++
} else {
    $erros++
}

# 2. Swagger
Write-Host "2️⃣ Verificando Swagger..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/swagger" -Method GET -TimeoutSec $TimeoutSeconds -UseBasicParsing
    if ($response.StatusCode -eq 200) {
        Write-Host "   ✅ Swagger UI acessível" -ForegroundColor Green
        $sucessos++
    } else {
        Write-Host "   ❌ Swagger UI não acessível (Status: $($response.StatusCode))" -ForegroundColor Red
        $erros++
    }
} catch {
    Write-Host "   ❌ Swagger UI não acessível: $($_.Exception.Message)" -ForegroundColor Red
    $erros++
}

# 3. Listar veículos (GET)
Write-Host "3️⃣ Testando GET /api/veiculo (listar)..." -ForegroundColor Yellow
if (Test-Endpoint -Method "GET" -Url "$BaseUrl/api/veiculo?page=1&pageSize=10" -Description "Listar veículos") {
    $sucessos++
} else {
    $erros++
}

# 4. Criar veículo (POST)
Write-Host "4️⃣ Testando POST /api/veiculo (criar)..." -ForegroundColor Yellow
$novoVeiculo = @{
    descricao = "Carro de teste - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    marca = 1
    modelo = "Teste Modelo"
    valor = 100000.00
} | ConvertTo-Json

if (Test-Endpoint -Method "POST" -Url "$BaseUrl/api/veiculo" -Body $novoVeiculo -Description "Criar veículo") {
    $sucessos++
    
    # Tentar obter o ID do veículo criado
    try {
        $veiculos = Invoke-RestMethod -Uri "$BaseUrl/api/veiculo?page=1&pageSize=1" -Method GET
        if ($veiculos.data -and $veiculos.data.Count -gt 0) {
            $veiculoId = $veiculos.data[0].id
            
            # 5. Obter veículo por ID (GET)
            Write-Host "5️⃣ Testando GET /api/veiculo/{id}..." -ForegroundColor Yellow
            if (Test-Endpoint -Method "GET" -Url "$BaseUrl/api/veiculo/$veiculoId" -Description "Obter veículo por ID") {
                $sucessos++
            } else {
                $erros++
            }
            
            # 6. Atualizar veículo (PUT)
            Write-Host "6️⃣ Testando PUT /api/veiculo/{id}..." -ForegroundColor Yellow
            $veiculoAtualizado = @{
                id = $veiculoId
                descricao = "Carro atualizado - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
                marca = 1
                modelo = "Teste Modelo Atualizado"
                valor = 120000.00
            } | ConvertTo-Json
            
            if (Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/veiculo/$veiculoId" -Body $veiculoAtualizado -Description "Atualizar veículo") {
                $sucessos++
            } else {
                $erros++
            }
            
            # 7. Deletar veículo (DELETE)
            Write-Host "7️⃣ Testando DELETE /api/veiculo/{id}..." -ForegroundColor Yellow
            try {
                $response = Invoke-WebRequest -Uri "$BaseUrl/api/veiculo/$veiculoId" -Method DELETE -TimeoutSec $TimeoutSeconds -UseBasicParsing
                if ($response.StatusCode -eq 204) {
                    Write-Host "   ✅ Deletar veículo" -ForegroundColor Green
                    $sucessos++
                } else {
                    Write-Host "   ❌ Deletar veículo (Status: $($response.StatusCode))" -ForegroundColor Red
                    $erros++
                }
            } catch {
                $statusCode = $_.Exception.Response.StatusCode.value__
                Write-Host "   ❌ Deletar veículo (Status: $statusCode)" -ForegroundColor Red
                $erros++
            }
        }
    } catch {
        Write-Host "   ⚠️ Não foi possível testar GET/PUT/DELETE (veículo pode não ter sido criado)" -ForegroundColor Yellow
    }
} else {
    $erros++
}

# Resumo
Write-Host ""
Write-Host "📊 Resumo dos Testes:" -ForegroundColor Cyan
Write-Host "   ✅ Sucessos: $sucessos" -ForegroundColor Green
Write-Host "   ❌ Erros: $erros" -ForegroundColor $(if ($erros -gt 0) { "Red" } else { "Green" })
Write-Host ""

if ($erros -eq 0) {
    Write-Host "🎉 Todos os testes passaram! A API está funcionando corretamente." -ForegroundColor Green
    Write-Host ""
    Write-Host "📚 Acesse o Swagger para mais testes:" -ForegroundColor Cyan
    Write-Host "   $BaseUrl/swagger" -ForegroundColor White
} else {
    Write-Host "⚠️ Alguns testes falharam. Verifique os logs da API." -ForegroundColor Yellow
}

Write-Host ""

