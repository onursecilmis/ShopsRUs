using ShopsRUs.Core.Data;

namespace ShopsRUs.Data.Domain;

public class Invoices : BaseEntity
{
    public bool IsGroceries { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
}