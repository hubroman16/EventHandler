using EventProcessor.Domain.Entity;

namespace EventProcessor.Producer.Interfaces;

public interface IMessageProducer
{
    void SendMessage(Event message);
}