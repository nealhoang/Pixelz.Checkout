namespace Pixelz.Application.Features.Orders.Dtos;

public class OrderItemDto
{
    public long Id { get; set; }

    public string ImageFileName { get; set; } = default!;

    public string RetouchType { get; set; } = default!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Total => UnitPrice * Quantity;
}
