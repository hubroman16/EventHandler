using EventProcessor.Application.Services;
using EventProcessor.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EventProcessor.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IEventProcessorService, EventProcessorService>();
    }
}