using MediatR;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Handlers;

public class ListarVeiculosHandler : IRequestHandler<ListarVeiculosQuery, PagedResult<Veiculo>>
{
    private readonly IVeiculoService _veiculoService;
    private readonly ILogger<ListarVeiculosHandler> _logger;

    public ListarVeiculosHandler(
        IVeiculoService veiculoService,
        ILogger<ListarVeiculosHandler> logger)
    {
        _veiculoService = veiculoService;
        _logger = logger;
    }

    public async Task<PagedResult<Veiculo>> Handle(ListarVeiculosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listando veículos com filtros - Page: {Page}, PageSize: {PageSize}, Marca: {Marca}, Modelo: {Modelo}",
            request.Page, request.PageSize, request.Marca, request.Modelo);
        
        var resultado = await _veiculoService.ListarComFiltrosAsync(
            request.Page,
            request.PageSize,
            request.Marca,
            request.Modelo,
            request.ValorMin,
            request.ValorMax,
            request.OrderBy,
            request.SortOrder);
        
        _logger.LogInformation("Total de veículos encontrados: {TotalCount} (Página {Page} de {TotalPages})",
            resultado.TotalCount, resultado.Page, resultado.TotalPages);

        return resultado;
    }
}

