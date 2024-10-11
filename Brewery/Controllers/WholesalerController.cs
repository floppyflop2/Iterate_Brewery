using BusinessLayer.Interface;
using DataLayer.Interface;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Brewery.Controllers;
[Route("[controller]")]
[ApiController]
public class WholesalerController(
    IWholesalerRepository wholesalerRepository,
    IValidator<Quote> quoteValidator,
    IQuoteService quoteService)
    : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Domain.Wholesaler>>> GetAll()
    {
        var wholesalers = await wholesalerRepository.GetAll();
        if (!wholesalers.Any()) return NoContent();
        return Ok(wholesalers);
    }

    // GET api/<WholesalerController>/5
    [HttpGet("{wholesalerId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Wholesaler>> GetAsync(int wholesalerId)
    {
        var wholesaler = await wholesalerRepository.GetById(wholesalerId);
        if (wholesaler == null) return NotFound(wholesalerId);
        return Ok(wholesaler);
    }

    // POST api/<WholesalerController>/{wholesalerId}/stock
    [HttpPost("{wholesalerId}/stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> AddStock(int wholesalerId, [FromBody] WholesalerStock stock)
    {
        var wholesaler = await wholesalerRepository.GetById(wholesalerId);
        if (wholesaler == null) return NotFound(wholesalerId);
        wholesaler.Stocks.Add(stock);
        await wholesalerRepository.Update(wholesaler);
        return Ok(wholesaler);
    }

    // PUT api/<WholesalerController>/stock/{beerId}/quantity
    [HttpPut("{wholesalerId}/stock/{beerId}/quantity={quantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> AddBeer(int wholesalerId, int beerId, int quantity)
    {
        var wholesaler = await wholesalerRepository.GetById(wholesalerId);
        if (wholesaler == null) return NotFound(wholesalerId);
        if (wholesaler.Stocks.Any(s => s.BeerId == beerId)) return NotFound(wholesalerId);
        wholesaler.Stocks.FirstOrDefault(stock => stock.BeerId == beerId)!.Quantity = quantity;
        await wholesalerRepository.Update(wholesaler);
        return Ok(wholesaler);
    }


    // PUT api/<WholesalerController>/stock/{beerId}/quantity
    [HttpPut("{wholesalerId}/quote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> CreateQuote(Quote quote, int wholesalerId)
    {
        ValidationResult results = await quoteValidator.ValidateAsync(quote);

        if (!results.IsValid)
        {
            return BadRequest(results.Errors);
        }
        var createdQuote = await quoteService.CreateQuote(quote.OrderItems, wholesalerId);
        return Ok(createdQuote);
    }
}
