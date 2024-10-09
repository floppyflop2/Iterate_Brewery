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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Domain.Brewery>>> GetAll()
    {
        var breweries = await _breweryRepository.GetAll();
        if (!breweries.Any()) return NoContent();
        return Ok(breweries);
    }

    // GET api/<BreweryController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Brewery>> GetAsync(int id)
    {
        var brewery = await _breweryRepository.GetById(id);
        if (brewery == null) return NotFound(id);
        return Ok(brewery);
    }

    // POST api/<BreweryController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromBody] Domain.Brewery brewery)
    {
        await _breweryRepository.Add(brewery);
        return Created("api/[controller]", brewery);
    }

    // POST api/<BreweryController>
    [HttpPost("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> AddBeer(int id, [FromBody] Domain.Beer beer)
    {
        var brewery = await _breweryRepository.GetById(id);
        if (brewery == null) return NotFound(id);
        brewery.Beers.Add(beer);
        await _breweryRepository.Update(brewery);
        return Ok(brewery);
    }

    // PUT api/<BreweryController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Brewery>> Put(int id, [FromBody] Domain.Brewery brewery)
    {
        var existingBrewery = await _breweryRepository.GetById(id);
        if (existingBrewery == null) return NotFound(id);

        await _breweryRepository.Update(brewery);
        var updatedBrewery = await _breweryRepository.GetById(id);

        return Ok(updatedBrewery);
    }
}
