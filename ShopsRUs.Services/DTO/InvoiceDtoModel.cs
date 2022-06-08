namespace ShopsRUs.Services.DTO;

public class InvoiceDtoModel
{
    public Guid Id { get; set; }

    public bool IsGroceries { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid CustomerId { get; set; }
    
    public decimal NetAmount { get; set; }
    
    public decimal DiscountAmount { get; set; }
}