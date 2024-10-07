using Brewery.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Brewery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BeersController : ControllerBase
    {
        private static readonly Beer[] Beers =
        [
            new Beer{ Name="beer1", AlcoholContent = 1.0, Price = 1.0}
        ];

        private readonly ILogger<BeersController> _logger;

        public BeersController(ILogger<BeersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Beer> GetAll()
        {
            return Beers.ToArray();
        }
    }
}
