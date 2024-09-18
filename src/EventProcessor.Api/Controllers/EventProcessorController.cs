using EventProcessor.Domain.Entity;
using EventProcessor.Domain.Interfaces;
using EventProcessor.Producer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventProcessor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventProcessorController: ControllerBase
{
    private readonly IMessageProducer _messageProducer;
    private readonly IEventProcessorService _eventProcessorService;

    public EventProcessorController(IMessageProducer messageProducer, IEventProcessorService eventProcessorService)
    {
        _messageProducer = messageProducer;
        _eventProcessorService = eventProcessorService;
    }

    [HttpPost]
    public IActionResult ProcessEvent([FromBody] Event @event)
    {
        try
        {
            Log.Information("Received event: {@Event}", @event);
            _messageProducer.SendMessage(@event);
            return Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error processing event: {@Event}", @event);
            throw;
        }
    }

    [HttpGet("incidents")]
    public async Task<IActionResult> GetIncidents(int page = 1, int pageSize = 10)
    {
        try
        {
            Log.Information("Fetching incidents. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var incidents = await _eventProcessorService.GetIncidentsAsync();
            var pagedIncidents = incidents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            Log.Information("Fetched {IncidentCount} incidents", pagedIncidents.Count);
            return Ok(pagedIncidents);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching incidents. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            throw;
        }
    }
}