namespace Pixelz.Infrastructure.Persistence.Factories;

public class PixelzWriteDbContextFactory : IDesignTimeDbContextFactory<PixelzWriteDbContext>
{
    public PixelzWriteDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)         
         .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PixelzWriteDbContext>();

        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found in appsettings.json.");

        optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

        return new PixelzWriteDbContext(optionsBuilder.Options);
    }
}
