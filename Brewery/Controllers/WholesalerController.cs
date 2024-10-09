using System.Net;
using DataLayer.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Brewery.Controllers;
[Route("[controller]")]
[ApiController]
public class WholesalerController : ControllerBase
{
    private readonly IWholesalerRepository _wholesalerRepository;
    private readonly ILogger<WholesalerController> _logger;

    public WholesalerController(IWholesalerRepository wholesalerRepository, ILogger<WholesalerController> logger)
    {
        _wholesalerRepository = wholesalerRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Domain.Wholesaler>>> GetAll()
    {
        var wholesalers = await _wholesalerRepository.GetAll();
        if (!wholesalers.Any()) return NoContent();
        return Ok(wholesalers);
    }

    // GET api/<WholesalerController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Wholesaler>> GetAsync(int id)
    {
        var wholesaler = await _wholesalerRepository.GetById(id);
        if (wholesaler == null) return NotFound(id);
        return Ok(wholesaler);
    }

    // POST api/<WholesalerController>/{id}/stock
    [HttpPost("{id}/stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> AddBeer(int id, [FromBody] WholesalerStock stock)
    {
        var wholesaler = await _wholesalerRepository.GetById(id);
        if (wholesaler == null) return NotFound(id);
        wholesaler.Stocks.Add(stock);
        await _wholesalerRepository.Update(wholesaler);
        return Ok(wholesaler);
    }

    // PUT api/<WholesalerController>/stock/{beerId}/quantity
    [HttpPut("{id}/stock/{beerId}/quantity={quantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> AddBeer(int id, int beerId, int quantity)
    {
        var wholesaler = await _wholesalerRepository.GetById(id);
        if (wholesaler == null) return NotFound(id);
        if (wholesaler.Stocks.Any(s => s.BeerId == beerId)) return NotFound(id);
        wholesaler.Stocks.FirstOrDefault(stock => stock.BeerId == beerId)!.Quantity = quantity;
        await _wholesalerRepository.Update(wholesaler);
        return Ok(wholesaler);
    }

}
