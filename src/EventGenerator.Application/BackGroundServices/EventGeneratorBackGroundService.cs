using System.Text;
using System.Text.Json;
using EventGenerator.Domain.Entity;
using EventGenerator.Domain.Enum;
using EventGenerator.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EventGenerator.Application.Services;

public class EventGeneratorBackGroundService : BackgroundService
{
    private readonly IHttpClientService _httpClient;
    private readonly IConfiguration _configuration;

    public EventGeneratorBackGroundService(IHttpClientService httpClientService, IConfiguration configuration)
    {
        _httpClient = httpClientService;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var baseAddress = _configuration.GetConnectionString("BaseAddress");

        while (!stoppingToken.IsCancellationRequested)
        {
            var eventgen = new Event
            {
                Id = Guid.NewGuid(),
                Type = (EventType)new Random().Next(1, 5),
                Time = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(eventgen);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Log.Information("Generating event: {@Event}", eventgen);

            await _httpClient.PostAsync($"{baseAddress}api/EventProcessor", content);

            var delay = new Random().Next(2000); 
            await Task.Delay(delay, stoppingToken);
        }
    }
}