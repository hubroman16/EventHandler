using System.Collections.Concurrent;
using EventProcessor.Domain.Entity;
using EventProcessor.Producer.Interfaces;
using Serilog;

namespace EventProcessor.Producer;

public class Producer: IMessageProducer
{
    private static readonly ConcurrentQueue<Event> _messageQueue = new ConcurrentQueue<Event>();

    public void SendMessage(Event message)
    {
        try
        {
            Log.Information("Enqueuing event: {@Event}", message);
            _messageQueue.Enqueue(message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error enqueuing event: {@Event}", message);
            throw;
        }
    }

    public static bool TryDequeue(out Event message)
    {
        try
        {
            var result = _messageQueue.TryDequeue(out message);
            if (result)
            {
                Log.Information("Dequeued event: {@Event}", message);
            }
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error dequeuing event");
            throw;
        }
    }
}