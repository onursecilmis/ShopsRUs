using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopsRUs.Data.Domain;

namespace ShopsRUs.Data.Mapping;

public class CustomersMap : BaseEntityTypeConfiguration<Customers>
{
    public override void Configure(EntityTypeBuilder<Customers> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerType);
        base.Configure(builder);
    }
}