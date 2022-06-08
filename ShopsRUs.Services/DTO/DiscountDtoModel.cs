namespace ShopsRUs.Services.DTO;

public class DiscountDtoModel
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public decimal DiscountRate { get; set; }
}