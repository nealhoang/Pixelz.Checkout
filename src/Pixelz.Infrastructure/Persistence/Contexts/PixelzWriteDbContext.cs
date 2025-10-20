namespace Pixelz.Infrastructure.Persistence.Contexts;

public class PixelzWriteDbContext : PixelzDbContextBase
{
    public PixelzWriteDbContext(DbContextOptions<PixelzWriteDbContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var utcNow = DateTime.UtcNow;
        var systemUserId = Guid.Empty.ToString();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
                if (string.IsNullOrWhiteSpace(entry.Entity.CreatedBy))
                {
                    entry.Entity.CreatedBy = systemUserId;
                }
                    
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = utcNow;
                if (string.IsNullOrWhiteSpace(entry.Entity.UpdatedBy))
                {
                    entry.Entity.UpdatedBy = systemUserId;
                }                    
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
