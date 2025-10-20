using Pixelz.Infrastructure.EventHandlers;

namespace Pixelz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {        
        string defaultConnectionString = config.GetConnectionString("Default")  
            ?? throw new InvalidOperationException("Default connection string is not configured.");

        string readConnectionString = config.GetConnectionString("ReadConnection") ?? defaultConnectionString;

        // Register DbContexts
        services.AddDbContext<PixelzWriteDbContext>(opt => opt.UseNpgsql(defaultConnectionString)
        .UseSnakeCaseNamingConvention());

        services.AddDbContext<PixelzReadDbContext>(opt => opt.UseNpgsql(readConnectionString)
        .UseSnakeCaseNamingConvention()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        //// Register repositories/services
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IOutboxService, OutboxService>();
       
        services.AddScoped<IIntegrationEventPublisher, MediatorIntegrationEventPublisher>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ProductionEventHandler).Assembly);
        });

        if (!AppDomain.CurrentDomain.FriendlyName.Contains("ef"))
        {
            services.AddScoped<IPaymentService, MockPaymentService>();
            services.AddScoped<IEmailService, MockEmailService>();
            services.AddScoped<IProductionService, MockProductionService>();
        }

        services.AddScoped<IUserContextService, CurrentUserService>();

        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
