namespace ShopsRUs.Core.Data;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedDateTime { get; set; }
    
    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? UpdatedDateTime { get; set; }
}