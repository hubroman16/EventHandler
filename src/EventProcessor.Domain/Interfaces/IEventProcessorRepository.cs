using EventProcessor.Domain.Entity;

namespace EventProcessor.Domain.Interfaces;

public interface IEventProcessorRepository
{
    Task AddIncidentAsync(Incident incident);
    Task<List<Incident>> GetIncidentsAsync();
    Task<List<Event>> GetRecentEventsAsync(DateTime startTime);
    Task AddRecentEventAsync(Event @event);
}