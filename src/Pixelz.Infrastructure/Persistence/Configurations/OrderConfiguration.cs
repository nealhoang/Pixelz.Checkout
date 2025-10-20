namespace Pixelz.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);

        builder.Property(o => o.OrderName).IsRequired().HasMaxLength(255);

        builder.Property(o => o.Status).HasConversion<byte>().IsRequired();

        builder.Property(o => o.TotalAmount).HasPrecision(10, 2);     

        builder.HasMany(o => o.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId);

        builder.HasMany(o => o.Payments)
               .WithOne(p => p.Order)
               .HasForeignKey(p => p.OrderId);

        builder.HasMany(o => o.Addresses)
               .WithOne(a => a.Order)
               .HasForeignKey(a => a.OrderId);

        builder.ConfigureAuditableEntity();
    }
}
