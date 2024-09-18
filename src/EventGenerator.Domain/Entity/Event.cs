using EventGenerator.Domain.Enum;

namespace EventGenerator.Domain.Entity;

public class Event
{
    public Guid Id { get; set; }
    public EventType Type { get; set; }
    public DateTime Time { get; set; }
}