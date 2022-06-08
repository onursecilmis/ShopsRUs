using ShopsRUs.Core.Data;

namespace ShopsRUs.Data.Domain;

public class Discounts : BaseEntity
{
    public string Type { get; set; }
    public decimal DiscountRate { get; set; }
}