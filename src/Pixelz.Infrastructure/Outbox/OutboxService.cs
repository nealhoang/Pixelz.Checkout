namespace Pixelz.Infrastructure.Outbox;

/// <summary>
/// Provides an implementation of the Outbox pattern.
/// Responsible for persisting integration events into the Outbox table,
/// to be published later by a background worker (OutboxProcessor).
/// </summary>
public class OutboxService : IOutboxService
{
    private readonly PixelzWriteDbContext _dbContext;

    public OutboxService(PixelzWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddEventAsync(IntegrationEvent @event, CancellationToken ct = default)
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = @event.OccurredOnUtc,
            Type = @event.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(
                @event,
                @event.GetType(),
                new JsonSerializerOptions { WriteIndented = false }
            ),
            ProcessedOnUtc = null
        };

        await _dbContext.OutboxMessages.AddAsync(outboxMessage, ct);
        // The actual SaveChanges is handled by the UnitOfWork
    }

    /// <inheritdoc />
    public async Task<List<OutboxMessage>> GetUnprocessedEventsAsync(CancellationToken ct = default)
    {
        return await _dbContext.OutboxMessages
            .Where(o => o.ProcessedOnUtc == null)
            .OrderBy(o => o.OccurredOnUtc)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task MarkAsProcessedAsync(Guid id, CancellationToken ct = default)
    {
        var message = await _dbContext.OutboxMessages.FirstOrDefaultAsync(o => o.Id == id, ct);
        if (message != null)
        {
            message.ProcessedOnUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(ct);
        }
    }

    /// <inheritdoc />
    public async Task IncrementRetryCountAsync(Guid messageId, int maxRetry, CancellationToken ct = default)
    {
        var msg = await _dbContext.OutboxMessages.FindAsync([messageId], ct);
        if (msg is null) return;

        msg.AttemptCount++;

        if (msg.AttemptCount >= maxRetry)
        {
            // Mark as dead-letter (no further retries)
            msg.ProcessedOnUtc = DateTime.UtcNow;
        }

        msg.LastErrorAtUtc = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc />
    public async Task RecordErrorAsync(Guid messageId, Exception ex, CancellationToken ct = default)
    {
        var msg = await _dbContext.OutboxMessages.FindAsync([messageId], ct);
        if (msg is null) return;

        msg.LastError = ex.Message.Length > 500
            ? ex.Message[..500] + "..."
            : ex.Message;

        msg.LastErrorAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);
    }
}
