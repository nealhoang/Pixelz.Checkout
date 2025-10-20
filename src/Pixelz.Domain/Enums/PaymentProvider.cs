namespace Pixelz.Domain.Enums;

/// <summary>
/// Represents the supported payment providers that can process a customer's checkout.
/// </summary>
/// <remarks>
/// This enum defines external payment service integrations.
/// Each value corresponds to a distinct payment gateway.
/// </remarks>
public enum PaymentProvider : byte
{
    /// <summary>
    /// A mock or simulated payment provider, used for testing or local development.
    /// </summary>
    MockPay = 1,

    /// <summary>
    /// Stripe payment gateway.
    /// </summary>
    Stripe = 2,

    /// <summary>
    /// PayPal payment service.
    /// </summary>
    PayPal = 3,

    /// <summary>
    /// Apple Pay — digital wallet and payment service by Apple.
    /// </summary>
    ApplePay = 4,

    /// <summary>
    /// Google Pay — digital wallet and payment service by Google.
    /// </summary>
    GooglePay = 5
}
