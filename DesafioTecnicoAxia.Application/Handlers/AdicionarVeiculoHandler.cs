using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Application.Common;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Handlers;

public class AdicionarVeiculoHandler : BaseHandler, IRequestHandler<AdicionarVeiculoCommand, Veiculo>
{
    private readonly IVeiculoService _veiculoService;
    private readonly IMapper _mapper;

    public AdicionarVeiculoHandler(
        IVeiculoService veiculoService,
        IMapper mapper,
        ILogger<AdicionarVeiculoHandler> logger) : base(logger)
    {
        _veiculoService = veiculoService;
        _mapper = mapper;
    }

    public async Task<Veiculo> Handle(AdicionarVeiculoCommand request, CancellationToken cancellationToken)
    {
        LogInformation("Adicionando novo veículo: {Modelo} - {Marca}", request.Modelo, request.Marca);

        var veiculo = Veiculo.Create(
            request.Descricao,
            request.Marca,
            request.Modelo,
            request.Opcionais,
            request.Valor);

        var resultado = await _veiculoService.AdicionarAsync(veiculo);

        LogInformation("Veículo adicionado com sucesso. Id: {Id}", resultado.Id);

        return resultado;
    }
}

