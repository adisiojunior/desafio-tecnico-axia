using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DesafioTecnicoAxia.Domain.Interfaces;
using DesafioTecnicoAxia.Infra.Context;

namespace DesafioTecnicoAxia.Infra.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        
        if (entity != null && typeof(T).GetProperty("IsDeleted") != null)
        {
            var isDeleted = (bool)(typeof(T).GetProperty("IsDeleted")?.GetValue(entity) ?? false);
            if (isDeleted)
                return null;
        }
        
        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        if (typeof(T).GetProperty("IsDeleted") != null)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "e");
            var property = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
            var constant = System.Linq.Expressions.Expression.Constant(false);
            var equal = System.Linq.Expressions.Expression.Equal(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equal, parameter);
            return await _dbSet.Where(lambda).ToListAsync();
        }
        
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        return entity != null;
    }
}

