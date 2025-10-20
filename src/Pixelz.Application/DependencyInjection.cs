namespace Pixelz.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Đăng ký MediatR cho Command / Query handlers
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Đăng ký FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Pipeline behavior: validation trước khi xử lý
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); 


        return services;
    }
}
