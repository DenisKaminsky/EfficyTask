using EfficyTask.Web.Settings;

namespace EfficyTask.Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}