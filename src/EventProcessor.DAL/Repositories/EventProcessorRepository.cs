using EventProcessor.Domain.Entity;
using EventProcessor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EventProcessor.DAL.Repositories;

public class EventProcessorRepository:IEventProcessorRepository
{
    private readonly ApplicationDbContext _context;
    private readonly List<Event> _recentEvents = new List<Event>();

    public EventProcessorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddIncidentAsync(Incident incident)
    {
        try
        {
            Log.Information("Adding incident to database: {@Incident}", incident);
            _context.Incidents.AddAsync(incident);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding incident to database: {@Incident}", incident);
            throw;
        }
    }

    public async Task<List<Incident>> GetIncidentsAsync()
    {
        try
        {
            Log.Information("Fetching incidents from database");
            return await _context.Incidents.Include(i => i.Events).ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching incidents from database");
            throw;
        }
    }
    
    public async Task<List<Event>> GetRecentEventsAsync(DateTime startTime)
    {
        try
        {
            Log.Information("Fetching recent events from database since {StartTime}", startTime);
            return await _context.Events
                .Where(e => e.Time >= startTime)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching recent events from database since {StartTime}", startTime);
            throw;
        }
    }
    
    public async Task AddRecentEventAsync(Event @event)
    {
        try
        {
            Log.Information("Adding recent event: {@Event}", @event);
            _recentEvents.Add(@event);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error adding recent event: {@Event}", @event);
            throw;
        }
    }
}