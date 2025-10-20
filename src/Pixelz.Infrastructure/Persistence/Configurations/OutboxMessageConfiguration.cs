namespace Pixelz.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="OutboxMessage"/> entity.
/// Ensures PostgreSQL column types, constraints, and indexes are properly defined.
/// </summary>
public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(o => o.Type)
            .HasColumnName("type")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(o => o.Content)
            .HasColumnName("content")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(o => o.OccurredOnUtc)
            .HasColumnName("occurred_on_utc")
            .IsRequired();

        builder.Property(o => o.ProcessedOnUtc)
            .HasColumnName("processed_on_utc");

        builder.Property(o => o.AttemptCount)
            .HasColumnName("attempt_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(o => o.LastError)
            .HasColumnName("last_error")
            .HasMaxLength(1000);

        builder.Property(o => o.LastErrorAtUtc)
            .HasColumnName("last_error_at_utc");

        builder.ConfigureAuditableEntity();

        builder.HasIndex(o => o.ProcessedOnUtc)
               .HasDatabaseName("ix_outbox_messages_processed");
    }
}
