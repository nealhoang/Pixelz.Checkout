namespace Pixelz.Infrastructure.Persistence.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("payment_transactions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Provider).HasMaxLength(50);

        builder.Property(p => p.TransactionId).HasMaxLength(100);

        builder.Property(p => p.Amount).HasPrecision(10, 2);

        builder.Property(p => p.Status).HasConversion<byte>();

        builder.ConfigureAuditableEntity();
    }
}
