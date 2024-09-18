using EventProcessor.Domain.Enum;

namespace EventProcessor.Domain.Entity;

public class Incident
{
    public Guid Id { get; set; }
    public IncidentType Type { get; set; }
    public DateTime Time { get; set; }
    public List<Event> Events { get; set; }
}