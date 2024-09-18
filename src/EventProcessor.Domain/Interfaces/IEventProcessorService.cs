using EventProcessor.Domain.Entity;

namespace EventProcessor.Domain.Interfaces;

public interface IEventProcessorService
{
    Task ProcessEventAsync(Event @event);
    Task<List<Incident>> GetIncidentsAsync();
}