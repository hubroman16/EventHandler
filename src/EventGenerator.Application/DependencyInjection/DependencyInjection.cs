using EventGenerator.Application.Services;
using EventGenerator.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventGenerator.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        InitServices(services, configuration);
    }

    private static void InitServices(this IServiceCollection services, IConfiguration configuration)
    {
        var baseAddress = configuration.GetConnectionString("BaseAddress");

        services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });

        services.AddScoped<IEventGeneratorService, EventGeneratorService>();
        services.AddHostedService<EventGeneratorBackGroundService>();
    }
}