using ShopsRUs.Core.Data;

namespace ShopsRUs.Data.Domain;

public class Customers : BaseEntity
{
  public string CustomerType { get; set; }
}