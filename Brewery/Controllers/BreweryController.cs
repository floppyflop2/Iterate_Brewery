using System.Net;
using DataLayer.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Brewery.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BreweryController : ControllerBase
{
    private readonly IBreweryRepository _breweryRepository;
    private readonly ILogger<BreweryController> _logger;

    public BreweryController(IBreweryRepository breweryRepository, ILogger<BreweryController> logger)
    {
        _breweryRepository = breweryRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<Domain.Brewery?>> Get()
    {
        return await _breweryRepository.GetAll();
    }

    // GET api/<BreweryController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var brewery = await _breweryRepository.GetById(id);
        if (brewery == null) return NotFound(id);
        return Ok(brewery);
    }

    // POST api/<BreweryController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Domain.Brewery brewery)
    {
        await _breweryRepository.Add(brewery);
        return Created("api/[controller]", brewery);
    }

    // POST api/<BreweryController>
    [HttpPost("{id}")]
    public async Task<IActionResult> AddBeer(int id, [FromBody] Domain.Beer beer)
    {
        var brewery = await _breweryRepository.GetById(id);
        if (brewery == null) return NotFound(id);
        brewery.Beers.Add(beer);
        await _breweryRepository.Update(brewery);
        return Ok(brewery);
    }

    // PUT api/<BreweryController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Domain.Brewery brewery)
    {
        await _breweryRepository.Update(brewery);
        return Ok(brewery);
    }
}
