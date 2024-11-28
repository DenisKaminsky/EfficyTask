using Efficy.Infrastructure.Data;

namespace EfficyTask.Web.Extensions;

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();

        await initializer.InitializeAsync();

        await initializer.SeedAsync();
    }
}