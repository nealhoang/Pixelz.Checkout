namespace Pixelz.Infrastructure.Persistence.Configurations;

public class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
{
    public void Configure(EntityTypeBuilder<OrderAddress> builder)
    {
        builder.ToTable("order_addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName).HasMaxLength(255).IsRequired();

        builder.Property(a => a.PhoneNumber).HasMaxLength(50).IsRequired();

        builder.Property(a => a.Line1).HasMaxLength(255).IsRequired();

        builder.Property(a => a.City).HasMaxLength(100).IsRequired();

        builder.Property(a => a.Country).HasMaxLength(100);

        builder.Property(a => a.Type)
               .HasConversion<byte>()
               .IsRequired();
    }
}
