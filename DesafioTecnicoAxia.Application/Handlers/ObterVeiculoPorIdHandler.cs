using MediatR;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Handlers;

public class ObterVeiculoPorIdHandler : IRequestHandler<ObterVeiculoPorIdQuery, Veiculo?>
{
    private readonly IVeiculoService _veiculoService;
    private readonly ILogger<ObterVeiculoPorIdHandler> _logger;

    public ObterVeiculoPorIdHandler(
        IVeiculoService veiculoService,
        ILogger<ObterVeiculoPorIdHandler> logger)
    {
        _veiculoService = veiculoService;
        _logger = logger;
    }

    public async Task<Veiculo?> Handle(ObterVeiculoPorIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando veículo com Id: {Id}", request.Id);
        
        var veiculo = await _veiculoService.ObterPorIdAsync(request.Id);
        
        if (veiculo == null)
        {
            _logger.LogWarning("Veículo com Id {Id} não encontrado", request.Id);
        }
        else
        {
            _logger.LogInformation("Veículo encontrado: {Modelo} - {Marca}", veiculo.Modelo, veiculo.Marca);
        }

        return veiculo;
    }
}

