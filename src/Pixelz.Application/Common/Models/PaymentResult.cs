namespace Pixelz.Application.Common.Models;

/// <summary>
/// Represents the result of a payment transaction.
/// This model is shared between Application and Infrastructure layers.
/// </summary>
public record PaymentResult(
    PaymentProvider Provider,
    string TransactionId,
    PaymentStatus Status,
    string? FailureReason = null
);

/// <summary>
/// Enumerates possible payment statuses.
/// </summary>
public enum PaymentStatus : byte
{
    /// <summary>
    /// Payment was successfully processed.
    /// </summary>
    Success = 0,

    /// <summary>
    /// Payment was rejected or declined.
    /// </summary>
    Failed = 1,

    /// <summary>
    /// Payment is still pending confirmation.
    /// </summary>
    Pending = 2
}
