using EfficyTask.Web.Settings;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Efficy.Application.Counters.Commands.CreateCounter;

namespace EfficyTask.Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Efficy Leaderboard API Documentation",
                Version = "v1",
                Description = "The use-case is a company-wide steps leaderboard application for teams of employees: " +
                              "picture that all employees are grouped into teams and issued step counters. " +
                              "This application needs to receive and store step count increments for each team, and calculate sums."
            });
            
            var webAssemblyXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, webAssemblyXmlFile));

            var applicationAssemblyXmlFile = $"{typeof(CreateCounterCommand).Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, applicationAssemblyXmlFile));

            options.DescribeAllParametersInCamelCase();
        });

        return services;
    }
}