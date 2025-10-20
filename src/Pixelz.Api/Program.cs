using Pixelz.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDefaultProblemDetails(builder.Environment);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<Pixelz.Infrastructure.Persistence.Contexts.PixelzWriteDbContext>();
        await Pixelz.Infrastructure.Persistence.Seed.DataSeeder.SeedAsync(db);
    }
}

app.UseRouting();

app.MapControllers();

app.MapHealthChecks("/health");

app.MapOrderEndpoints();

app.Run();
