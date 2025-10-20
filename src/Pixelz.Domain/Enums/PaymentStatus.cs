namespace Pixelz.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of a payment transaction.
/// </summary>
/// <remarks>
/// This enum describes the possible outcomes of a payment attempt,
/// from initial processing to completion or refund.
/// </remarks>
public enum PaymentStatus : byte
{
    /// <summary>
    /// The payment request has been created but not yet processed.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The payment has been successfully processed and confirmed by the provider.
    /// </summary>
    Success = 1,

    /// <summary>
    /// The payment attempt has failed due to decline, timeout, or provider error.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// The payment was successfully processed earlier but has been refunded.
    /// </summary>
    Refunded = 3
}
