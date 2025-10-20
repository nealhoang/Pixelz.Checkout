namespace Pixelz.Infrastructure.Persistence.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void ConfigureAuditableEntity<T>(this EntityTypeBuilder<T> builder)
        where T : AuditableEntity
    {
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(36)
            .IsRequired()
            .HasDefaultValue(Guid.Empty.ToString());

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(36)
            .IsRequired(false);

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);
    }
}
