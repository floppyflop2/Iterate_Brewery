using DataLayer.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Brewery.Controllers;

[ApiController]
[Route("[controller]")]
public class BeersController : ControllerBase
{
    private readonly IBeerRepository _beerRepository;
    private readonly ILogger<BeersController> _logger;

    public BeersController(IBeerRepository beerRepository, ILogger<BeersController> logger)
    {
        _beerRepository = beerRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var beers = await _beerRepository.GetAll();
        return Ok(beers);
    }
}