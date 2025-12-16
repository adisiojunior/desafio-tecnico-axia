using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Application.Common;
using DesafioTecnicoAxia.Application.Common.Exceptions;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Handlers;

public class AtualizarVeiculoHandler : BaseHandler, IRequestHandler<AtualizarVeiculoCommand, Veiculo>
{
    private readonly IVeiculoService _veiculoService;
    private readonly IMapper _mapper;

    public AtualizarVeiculoHandler(
        IVeiculoService veiculoService,
        IMapper mapper,
        ILogger<AtualizarVeiculoHandler> logger) : base(logger)
    {
        _veiculoService = veiculoService;
        _mapper = mapper;
    }

    public async Task<Veiculo> Handle(AtualizarVeiculoCommand request, CancellationToken cancellationToken)
    {
        LogInformation("Atualizando veículo com Id: {Id}", request.Id);

        var veiculoExistente = await _veiculoService.ObterPorIdAsync(request.Id);
        
        if (veiculoExistente == null)
        {
            LogWarning("Veículo com Id {Id} não encontrado para atualização", request.Id);
            throw new NotFoundException(nameof(Veiculo), request.Id);
        }

        veiculoExistente.Update(
            request.Descricao,
            request.Marca,
            request.Modelo,
            request.Opcionais,
            request.Valor);

        var resultado = await _veiculoService.AtualizarAsync(veiculoExistente);

        LogInformation("Veículo atualizado com sucesso. Id: {Id}", resultado.Id);

        return resultado;
    }
}

