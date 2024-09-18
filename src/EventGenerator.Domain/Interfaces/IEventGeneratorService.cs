using EventGenerator.Domain.Entity;

namespace EventGenerator.Domain.Interfaces;

public interface IEventGeneratorService
{
    Task<Event> GenerateEvent();
}