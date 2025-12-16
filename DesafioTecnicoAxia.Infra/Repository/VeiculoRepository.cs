using System.Linq.Expressions;
using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Domain.Entidades;
using DesafioTecnicoAxia.Domain.Enumeradores;
using DesafioTecnicoAxia.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAxia.Infra.Repository;

public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
{
    public VeiculoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<Veiculo>> ListarComFiltrosAsync(
        int page,
        int pageSize,
        Marca? marca,
        string? modelo,
        decimal? valorMin,
        decimal? valorMax,
        string? orderBy,
        string? sortOrder)
    {
        // Validar e limitar pageSize
        pageSize = Math.Min(Math.Max(pageSize, 1), 100);
        page = Math.Max(page, 1);
        var query = _dbSet.Where(v => !v.IsDeleted);

        if (marca.HasValue)
        {
            query = query.Where(v => v.Marca == marca.Value);
        }

        if (!string.IsNullOrWhiteSpace(modelo))
        {
            query = query.Where(v => v.Modelo.Contains(modelo, StringComparison.OrdinalIgnoreCase));
        }

        if (valorMin.HasValue)
        {
            query = query.Where(v => v.Valor.HasValue && v.Valor >= valorMin.Value);
        }

        if (valorMax.HasValue)
        {
            query = query.Where(v => v.Valor.HasValue && v.Valor <= valorMax.Value);
        }

        var totalCount = await query.CountAsync();
        query = ApplyOrderBy(query, orderBy ?? "CreatedAt", sortOrder ?? "desc");
        var skip = (page - 1) * pageSize;
        var data = await query.Skip(skip).Take(pageSize).ToListAsync();

        return new PagedResult<Veiculo>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private IQueryable<Veiculo> ApplyOrderBy(IQueryable<Veiculo> query, string orderBy, string sortOrder)
    {
        var isDescending = sortOrder?.ToLower() == "desc";

        return orderBy?.ToLower() switch
        {
            "marca" => isDescending ? query.OrderByDescending(v => v.Marca) : query.OrderBy(v => v.Marca),
            "modelo" => isDescending ? query.OrderByDescending(v => v.Modelo) : query.OrderBy(v => v.Modelo),
            "valor" => isDescending ? query.OrderByDescending(v => v.Valor ?? 0) : query.OrderBy(v => v.Valor ?? 0),
            "createdat" => isDescending ? query.OrderByDescending(v => v.CreatedAt) : query.OrderBy(v => v.CreatedAt),
            "updatedat" => isDescending ? query.OrderByDescending(v => v.UpdatedAt ?? DateTime.MinValue) : query.OrderBy(v => v.UpdatedAt ?? DateTime.MinValue),
            _ => isDescending ? query.OrderByDescending(v => v.CreatedAt) : query.OrderBy(v => v.CreatedAt)
        };
    }
}

