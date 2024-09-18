using Microsoft.Extensions.DependencyInjection;

namespace EventProcessor.Consumer.DependencyInjection;

public static class DependencyInjection
{
    public static void AddConsumer(this IServiceCollection services)
    {
        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddHostedService<Consumer>();
    }
}