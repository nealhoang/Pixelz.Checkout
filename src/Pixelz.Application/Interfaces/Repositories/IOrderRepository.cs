namespace Pixelz.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id, CancellationToken ct = default);

    Task<PagedResult<Order>> SearchByNameAsync(string? name, int pageIndex = 1, int pageSize = 15, CancellationToken ct = default);

    Task AddAsync(Order order, CancellationToken ct = default);

    Task UpdateAsync(Order order, CancellationToken ct = default);
}
