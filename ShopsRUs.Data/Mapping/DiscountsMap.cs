using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopsRUs.Data.Domain;

namespace ShopsRUs.Data.Mapping;

public class DiscountsMap : BaseEntityTypeConfiguration<Discounts>
{
    public override void Configure(EntityTypeBuilder<Discounts> builder)
    {
        builder.ToTable("Discounts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.DiscountRate).IsRequired();
        base.Configure(builder);
    }
}