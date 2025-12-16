using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.VeiculoService;

public interface IVeiculoService
{
    Task<Veiculo> AdicionarAsync(Veiculo veiculo);
    Task<Veiculo> AtualizarAsync(Veiculo veiculo);
    Task<Veiculo?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Veiculo>> ListarAsync();
    Task<PagedResult<Veiculo>> ListarComFiltrosAsync(
        int page,
        int pageSize,
        Domain.Enumeradores.Marca? marca,
        string? modelo,
        decimal? valorMin,
        decimal? valorMax,
        string? orderBy,
        string? sortOrder);
    Task<bool> ExcluirAsync(Guid id);
}

