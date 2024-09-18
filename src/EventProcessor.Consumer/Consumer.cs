using EventProcessor.Consumer.Interfaces;
using EventProcessor.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EventProcessor.Consumer;

public class Consumer : BackgroundService, IMessageConsumer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Consumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Log.Information("Consumer service started");
            await StartConsumingAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Consumer service error");
            throw;
        }
    }

    public async Task StartConsumingAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Producer.Producer.TryDequeue(out var @event))
                {
                    Log.Information("Processing event: {@Event}", @event);
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var eventProcessorService = scope.ServiceProvider.GetRequiredService<IEventProcessorService>();
                        await eventProcessorService.ProcessEventAsync(@event);
                    }
                }
                else
                {
                    await Task.Delay(100, stoppingToken); 
                }
            }
            Log.Information("Consumer service stopped");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error consuming event");
            throw;
        }
    }
}