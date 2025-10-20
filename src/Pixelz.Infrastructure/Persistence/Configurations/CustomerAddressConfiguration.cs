namespace Pixelz.Infrastructure.Persistence.Configurations;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("customer_addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName).HasMaxLength(255).IsRequired();

        builder.Property(a => a.PhoneNumber).HasMaxLength(50).IsRequired();

        builder.Property(a => a.Line1).HasMaxLength(255).IsRequired();

        builder.Property(a => a.City).HasMaxLength(100).IsRequired();

        builder.Property(a => a.State).HasMaxLength(100);

        builder.Property(a => a.Country).HasMaxLength(100);

        builder.Property(a => a.PostalCode).HasMaxLength(20);

        builder.Property(a => a.Type)
               .HasConversion<byte>()
               .IsRequired();

        builder.ConfigureAuditableEntity();
    }
}
