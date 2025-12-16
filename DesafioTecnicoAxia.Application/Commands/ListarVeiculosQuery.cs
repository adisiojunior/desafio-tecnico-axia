using MediatR;
using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Commands;

public class ListarVeiculosQuery : IRequest<PagedResult<Veiculo>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Domain.Enumeradores.Marca? Marca { get; set; }
    public string? Modelo { get; set; }
    public decimal? ValorMin { get; set; }
    public decimal? ValorMax { get; set; }
    public string? OrderBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}

