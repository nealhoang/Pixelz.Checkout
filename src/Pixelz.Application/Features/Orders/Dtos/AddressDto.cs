namespace Pixelz.Application.Features.Orders.Dtos;

public class AddressDto
{
    public long Id { get; set; }

    public string FullName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Line1 { get; set; } = default!;

    public string? Line2 { get; set; }

    public string City { get; set; } = default!;

    public string State { get; set; } = default!;

    public string Country { get; set; } = default!;

    public string PostalCode { get; set; } = default!;

    public string Type { get; set; } = default!;
}
