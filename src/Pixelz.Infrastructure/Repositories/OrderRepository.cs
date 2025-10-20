namespace Pixelz.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly PixelzReadDbContext _readDb;
    private readonly PixelzWriteDbContext _writeDb;    

    public OrderRepository(PixelzReadDbContext readDb, PixelzWriteDbContext writeDb)
    {
        _readDb = readDb;
        _writeDb = writeDb;
    }

    public async Task<Order?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _readDb.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task<PagedResult<Order>> SearchByNameAsync(string? name, int pageIndex = 1, int pageSize = 15, CancellationToken ct = default)
    {
        if (pageIndex <= 0)
        {
            pageIndex = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 15;
        }

        if (pageSize > 100)
        {
            pageSize = 100;
        }

        IQueryable<Order> query = _readDb.Orders
            .Include(o => o.Customer)
            .OrderByDescending(o => o.CreatedAt);

        if (!string.IsNullOrWhiteSpace(name))
        {
            var keyword = name.Trim().ToLower();
            query = query.Where(o => EF.Functions.ILike(o.OrderName, $"%{keyword}%"));
        }

        var totalCount = await query.CountAsync(ct);
        var skip = (pageIndex - 1) * pageSize;

        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Order>(items, totalCount, pageIndex, pageSize);
    }

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _writeDb.Orders.AddAsync(order, ct);
    }

    public Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _writeDb.Orders.Update(order);

        return Task.CompletedTask;
    }
}
