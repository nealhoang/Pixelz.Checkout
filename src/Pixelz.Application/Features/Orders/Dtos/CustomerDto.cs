namespace Pixelz.Application.Features.Orders.Dtos;

public class CustomerDto
{
    public long Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
}
