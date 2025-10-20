namespace Pixelz.Application.Features.Orders.Queries.SearchOrders;

public record SearchOrdersQuery(string? Keyword, int PageIndex = 1, int PageSize = 15) : IRequest<PagedResult<OrderDto>>;
