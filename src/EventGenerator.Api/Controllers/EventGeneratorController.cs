using EventGenerator.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventGenerator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventGeneratorController : ControllerBase
{
    private readonly IEventGeneratorService _generatorService;

    public EventGeneratorController(IEventGeneratorService generatorService)
    {
        _generatorService = generatorService;
    }
    
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateEvent()
    {
        Log.Information("Received request to generate event");

        var eventgen = await _generatorService.GenerateEvent();

        Log.Information("Generated event: {@Event}", eventgen);

        return Ok(eventgen);
    }
}