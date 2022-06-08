using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopsRUs.Data.Domain;

namespace ShopsRUs.Data.Mapping;

public class InvoicesMap : BaseEntityTypeConfiguration<Invoices>
{
    public override void Configure(EntityTypeBuilder<Invoices> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.IsGroceries).IsRequired();
        builder.Property(x => x.TotalAmount).IsRequired();
        base.Configure(builder);
    }
}