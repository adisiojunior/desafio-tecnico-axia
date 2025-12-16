using MediatR;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Application.Common.Exceptions;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Handlers;

public class ExcluirVeiculoHandler : IRequestHandler<ExcluirVeiculoCommand, bool>
{
    private readonly IVeiculoService _veiculoService;
    private readonly ILogger<ExcluirVeiculoHandler> _logger;

    public ExcluirVeiculoHandler(
        IVeiculoService veiculoService,
        ILogger<ExcluirVeiculoHandler> logger)
    {
        _veiculoService = veiculoService;
        _logger = logger;
    }

    public async Task<bool> Handle(ExcluirVeiculoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Excluindo veículo com Id: {Id}", request.Id);

        var veiculoExistente = await _veiculoService.ObterPorIdAsync(request.Id);
        
        if (veiculoExistente == null)
        {
            _logger.LogWarning("Veículo com Id {Id} não encontrado para exclusão", request.Id);
            throw new NotFoundException(nameof(Veiculo), request.Id);
        }

        var resultado = await _veiculoService.ExcluirAsync(request.Id);

        if (resultado)
        {
            _logger.LogInformation("Veículo excluído com sucesso. Id: {Id}", request.Id);
        }

        return resultado;
    }
}

