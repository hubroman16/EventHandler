using System.Text;
using System.Text.Json;
using EventGenerator.Domain.Entity;
using EventGenerator.Domain.Enum;
using EventGenerator.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EventGenerator.Application.Services;

public class EventGeneratorService : IEventGeneratorService
{
    private readonly IHttpClientService _httpClientService;
    private readonly IConfiguration _configuration;

    public EventGeneratorService(IHttpClientService httpClientService, IConfiguration configuration)
    {
        _httpClientService = httpClientService;
        _configuration = configuration;
    }

    public async Task<Event> GenerateEvent()
    {
        var baseAddress = _configuration.GetConnectionString("BaseAddress");
        var eventgen = new Event
        {
            Id = Guid.NewGuid(),
            Type = (EventType)new Random().Next(1, 5),
            Time = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(eventgen);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        Log.Information("Generating event: {@Event}", eventgen);

        await _httpClientService.PostAsync($"{baseAddress}api/EventProcessor", content);
        return eventgen;
    }
}