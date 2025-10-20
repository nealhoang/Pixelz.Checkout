namespace Pixelz.Infrastructure.Persistence.Contexts;

public class PixelzReadDbContext : PixelzDbContextBase
{
    public PixelzReadDbContext(DbContextOptions<PixelzReadDbContext> options): base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}
