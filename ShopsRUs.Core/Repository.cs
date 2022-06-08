using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Core.Data;

namespace ShopsRUs.Core;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }


    public async Task<Guid> AddUpdateAsync(TEntity entity, Guid logUserId)
    {
        var obj = await DetailAsync(entity.Id);
        if (obj == null)
        {
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedDateTime = DateTime.UtcNow;
            await _context.Set<TEntity>().AddAsync(entity);
        }
        else
        {
            entity.IsActive = true;
            entity.UpdatedDateTime = new DateTime?(DateTime.UtcNow);
            _context.Entry(entity).State = EntityState.Modified;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Guid.Empty;
        }

        return entity.Id;
    }

    public async Task<TEntity?> DetailAsync(Guid id)
    {
        try
        {
            return await _context.Set<TEntity>().Where(w => w.Id == id && w.IsActive == true).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Where(predicate).AsQueryable();
    }

    public FilterModel<TEntity> Filter(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, Expression<Func<TEntity, object>>? shortField, bool sortDescending)
    {
        var returnVal = new FilterModel<TEntity>();

        if (shortField == null)
        {
            shortField = arg => arg.Id;
            sortDescending = true;
        }

        if (page == 0)
        {
            page = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 10;
        }

        var skip = (page - 1) * pageSize;
        var filter = _context.Set<TEntity>().Where(predicate).AsQueryable();
        returnVal.Total = filter.AsNoTracking().Count();
        filter = sortDescending ? filter.OrderByDescending(shortField) : filter.OrderBy(shortField);
        filter = filter.Skip(skip).Take(pageSize);
        returnVal.Data = filter.AsNoTracking().AsQueryable();

        return returnVal;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid logUserId)
    {
        var entity = await DetailAsync(id);
        if (entity == null)
        {
            return false;
        }

        entity.IsActive = false;
        entity.IsDeleted = true;
        entity.UpdatedDateTime = new DateTime?(DateTime.UtcNow);
        return await _context.SaveChangesAsync() > 0;
    }
}