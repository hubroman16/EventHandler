using EventProcessor.Domain.Enum;

namespace EventProcessor.Domain.Entity;

public class Event
{
    public Guid Id { get; set; }
    public EventType Type { get; set; }
    public DateTime Time { get; set; }
}