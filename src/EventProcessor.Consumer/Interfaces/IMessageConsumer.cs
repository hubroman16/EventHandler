namespace EventProcessor.Consumer.Interfaces;

public interface IMessageConsumer
{
    Task StartConsumingAsync(CancellationToken stoppingToken);
}