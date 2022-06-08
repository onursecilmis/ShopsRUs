using ShopsRUs.Core.Data;

namespace ShopsRUs.Core;

public class FilterModel<T> where T : BaseEntity
{
    
    public int Total { get; set; }
    
    public IQueryable<T> Data { get; set; }
}