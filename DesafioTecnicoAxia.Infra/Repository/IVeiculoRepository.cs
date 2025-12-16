using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Domain.Entidades;
using DesafioTecnicoAxia.Domain.Interfaces;

namespace DesafioTecnicoAxia.Infra.Repository;

public interface IVeiculoRepository : IRepository<Veiculo>
{
    Task<PagedResult<Veiculo>> ListarComFiltrosAsync(
        int page,
        int pageSize,
        Domain.Enumeradores.Marca? marca,
        string? modelo,
        decimal? valorMin,
        decimal? valorMax,
        string? orderBy,
        string? sortOrder);
}

