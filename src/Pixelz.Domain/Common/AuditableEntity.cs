namespace Pixelz.Domain.Common;

/// <summary>
/// Represents the base class for all auditable entities,
/// providing metadata for creation and modification tracking.
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// Identifier of the user who created the record.
    /// </summary>
    public string CreatedBy { get; set; } = Guid.Empty.ToString();

    /// <summary>
    /// Timestamp (UTC) when the record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identifier of the user who last modified the record.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Timestamp (UTC) when the record was last modified.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
