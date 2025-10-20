namespace Pixelz.Application.Features.Orders.Commands.CheckoutOrder;

public record CheckoutOrderCommand(long OrderId) : IRequest<bool>;
