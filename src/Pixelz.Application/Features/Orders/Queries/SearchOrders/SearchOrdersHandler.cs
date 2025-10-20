namespace Pixelz.Application.Features.Orders.Queries.SearchOrders;

public class SearchOrdersHandler : IRequestHandler<SearchOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IMapper _mapper;

    private readonly IOrderRepository _orderRepository;

    public SearchOrdersHandler(IMapper mapper, IOrderRepository orderRepository)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<OrderDto>> Handle(SearchOrdersQuery request, CancellationToken ct)
    {
        PagedResult<Order> orders = await _orderRepository.SearchByNameAsync(request.Keyword, request.PageIndex, request.PageSize, ct);

        return _mapper.Map<PagedResult<OrderDto>>(orders);
    }
}
