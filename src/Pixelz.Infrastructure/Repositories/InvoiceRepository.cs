namespace Pixelz.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly PixelzWriteDbContext _dbContext;

    public InvoiceRepository(PixelzWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        await _dbContext.Invoices.AddAsync(invoice, ct);
    }
}