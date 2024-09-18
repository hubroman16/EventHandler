using EventProcessor.Domain.Entity;
using EventProcessor.Domain.Enum;
using EventProcessor.Domain.Interfaces;
using Serilog;

namespace EventProcessor.Application.Services;

public class EventProcessorService : IEventProcessorService
{
    private readonly IEventProcessorRepository _eventProcessorRepository;

    public EventProcessorService(IEventProcessorRepository eventProcessorRepository)
    {
        _eventProcessorRepository = eventProcessorRepository;
    }

    public async Task ProcessEventAsync(Event @event)
    {
        try
        {
            Log.Information("Processing event: {@Event}", @event);

            var incident = await CreateIncidentAsync(@event);

            if (incident != null)
            {
                Log.Information("Created incident: {@Incident}", incident);
                await _eventProcessorRepository.AddIncidentAsync(incident);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing event: {@Event}", @event);
            throw;
        }
    }

    public async Task<List<Incident>> GetIncidentsAsync()
    {
        try
        {
            Log.Information("Fetching incidents");
            return await _eventProcessorRepository.GetIncidentsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching incidents");
            throw;
        }
    }
    
    private async Task<Incident> CreateIncidentAsync(Event @event)
    {
        try
        {
            if (@event.Type == EventType.Event1)
            {
                var recentEvents = await _eventProcessorRepository.GetRecentEventsAsync(DateTime.UtcNow.AddSeconds(-20));
                
                if (recentEvents.Any(e => e.Type == EventType.Event2))
                {
                    var incident = new Incident
                    {
                        Id = Guid.NewGuid(),
                        Type = IncidentType.Composite,
                        Time = DateTime.UtcNow,
                        Events = recentEvents.Concat(new List<Event> { @event }).ToList()
                    };
                    Log.Information("Created composite incident: {@Incident}", incident);
                    return incident;
                }
                else
                {
                    var incident = new Incident
                    {
                        Id = Guid.NewGuid(),
                        Type = IncidentType.Simple,
                        Time = DateTime.UtcNow,
                        Events = new List<Event> { @event }
                    };
                    Log.Information("Created simple incident: {@Incident}", incident);
                    return incident;
                }
            }
            else if (@event.Type == EventType.Event2)
            {
                Log.Information("Adding recent event: {@Event}", @event);
                await _eventProcessorRepository.AddRecentEventAsync(@event);
            }

            return null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating incident for event: {@Event}", @event);
            throw;
        }
    }
}