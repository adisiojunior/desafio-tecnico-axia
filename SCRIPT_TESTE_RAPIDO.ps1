# Script PowerShell para criar múltiplos veículos rapidamente
# Execute: .\SCRIPT_TESTE_RAPIDO.ps1

$baseUrl = "http://localhost:8080/api/veiculo"

$veiculos = @(
    @{
        descricao = "Carro esportivo em excelente estado, único dono, revisões em dia"
        marca = 1
        modelo = "Mustang GT"
        opcionais = "Ar condicionado, Vidros elétricos, Sistema de som premium, Teto solar"
        valor = 250000.00
    },
    @{
        descricao = "SUV familiar espaçosa, ideal para viagens"
        marca = 2
        modelo = "Trailblazer"
        opcionais = "Ar condicionado, GPS, Câmera de ré, Bancos de couro"
        valor = 180000.00
    },
    @{
        descricao = "Carro popular econômico, perfeito para cidade"
        marca = 3
        modelo = "Uno"
        opcionais = $null
        valor = 45000.00
    },
    @{
        descricao = "Sedan confortável e elegante, completo"
        marca = 4
        modelo = "Jetta"
        opcionais = "Ar condicionado, Bancos de couro, Sensor de estacionamento"
        valor = 120000.00
    },
    @{
        descricao = "SUV robusta e confiável, excelente para estradas"
        marca = 5
        modelo = "Hilux"
        opcionais = "Ar condicionado, GPS, Câmera de ré, Bancos de couro, Barra de proteção"
        valor = 220000.00
    },
    @{
        descricao = "Sedan esportivo, motor potente"
        marca = 6
        modelo = "Civic"
        opcionais = "Ar condicionado, GPS, Bancos de couro, Rodas de liga leve"
        valor = 150000.00
    },
    @{
        descricao = "SUV compacta, ideal para cidade"
        marca = 7
        modelo = "Kicks"
        opcionais = "Ar condicionado, GPS, Câmera de ré"
        valor = 95000.00
    },
    @{
        descricao = "Hatchback moderno e econômico"
        marca = 8
        modelo = "HB20"
        opcionais = "Ar condicionado, Direção elétrica"
        valor = 65000.00
    },
    @{
        descricao = "Sedan francês, confortável e elegante"
        marca = 9
        modelo = "Fluence"
        opcionais = "Ar condicionado, GPS, Bancos de couro"
        valor = 85000.00
    },
    @{
        descricao = "SUV francesa, espaçosa e confortável"
        marca = 10
        modelo = "3008"
        opcionais = "Ar condicionado, GPS, Câmera de ré, Bancos de couro, Teto solar"
        valor = 140000.00
    }
)

Write-Host "🚗 Criando veículos..." -ForegroundColor Cyan

$criados = 0
$erros = 0

foreach ($veiculo in $veiculos) {
    try {
        $body = @{
            descricao = $veiculo.descricao
            marca = $veiculo.marca
            modelo = $veiculo.modelo
            opcionais = $veiculo.opcionais
            valor = $veiculo.valor
        } | ConvertTo-Json

        $response = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
        
        Write-Host "✅ Criado: $($veiculo.modelo) - ID: $($response.id)" -ForegroundColor Green
        $criados++
        
        Start-Sleep -Milliseconds 200  # Pequeno delay entre requisições
    }
    catch {
        Write-Host "❌ Erro ao criar $($veiculo.modelo): $($_.Exception.Message)" -ForegroundColor Red
        $erros++
    }
}

Write-Host "`n📊 Resumo:" -ForegroundColor Yellow
Write-Host "   Criados: $criados" -ForegroundColor Green
Write-Host "   Erros: $erros" -ForegroundColor $(if ($erros -gt 0) { "Red" } else { "Green" })

Write-Host "`n🔍 Teste a listagem:" -ForegroundColor Cyan
Write-Host "   GET $baseUrl?page=1&pageSize=10" -ForegroundColor White

