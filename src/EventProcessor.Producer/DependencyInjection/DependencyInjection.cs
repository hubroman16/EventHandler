using EventProcessor.Producer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EventProcessor.Producer.DependencyInjection;

public static class DependencyInjection
{
    public static void AddProducer(this IServiceCollection services)
    {
        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageProducer, Producer>();
    }
}