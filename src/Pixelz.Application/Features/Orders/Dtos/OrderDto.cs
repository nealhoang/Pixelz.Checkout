namespace Pixelz.Application.Features.Orders.Dtos;

public class OrderDto
{
    public long Id { get; set; }

    public string OrderNumber { get; set; } = default!;

    public string OrderName { get; set; } = default!;

    public string Status { get; set; } = default!;

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public CustomerDto Customer { get; set; } = default!;

    public List<OrderItemDto> Items { get; set; } = new();

    public List<AddressDto> Addresses { get; set; } = new();
}
