namespace Pixelz.Infrastructure.Persistence.Contexts;

public class PixelzDbContextBase : DbContext
{
    protected PixelzDbContextBase(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<OrderAddress> OrderAddresses => Set<OrderAddress>();

    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();

    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {       
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PixelzDbContextBase).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
