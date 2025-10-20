namespace Pixelz.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImageFileName).HasMaxLength(255);

        builder.Property(i => i.RetouchType).HasMaxLength(100);

        builder.Property(i => i.UnitPrice).HasPrecision(10, 2);

        builder.ConfigureAuditableEntity();
    }
}
