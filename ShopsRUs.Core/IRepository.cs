using System.Linq.Expressions;
using ShopsRUs.Core.Data;
namespace ShopsRUs.Core;

public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<Guid> AddUpdateAsync(TEntity entity, Guid logUserId);
    
    Task<TEntity?> DetailAsync(Guid id);
    
    IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
    
    FilterModel<TEntity> Filter(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, Expression<Func<TEntity, object>>? shortField, bool sortDescending);
    
    Task<bool> DeleteAsync(Guid id, Guid logUserId);
}