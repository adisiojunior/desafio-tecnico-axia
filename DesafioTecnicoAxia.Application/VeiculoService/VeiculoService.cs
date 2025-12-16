using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Domain.Entidades;
using DesafioTecnicoAxia.Infra.Repository;

namespace DesafioTecnicoAxia.Application.VeiculoService;

public class VeiculoService : IVeiculoService
{
    private readonly IVeiculoRepository _repository;

    public VeiculoService(IVeiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Veiculo> AdicionarAsync(Veiculo veiculo)
    {
        return await _repository.AddAsync(veiculo);
    }

    public async Task<Veiculo> AtualizarAsync(Veiculo veiculo)
    {
        return await _repository.UpdateAsync(veiculo);
    }

    public async Task<Veiculo?> ObterPorIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Veiculo>> ListarAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<PagedResult<Veiculo>> ListarComFiltrosAsync(
        int page,
        int pageSize,
        Domain.Enumeradores.Marca? marca,
        string? modelo,
        decimal? valorMin,
        decimal? valorMax,
        string? orderBy,
        string? sortOrder)
    {
        return await _repository.ListarComFiltrosAsync(
            page, pageSize, marca, modelo, valorMin, valorMax, orderBy, sortOrder);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var veiculo = await _repository.GetByIdAsync(id);
        if (veiculo == null)
            return false;

        veiculo.MarkAsDeleted();
        await _repository.UpdateAsync(veiculo);
        return true;
    }
}

