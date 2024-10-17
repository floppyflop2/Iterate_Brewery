using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class BeerRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<Beer>(breweryDbContext), IBeerRepository
{
    private readonly BreweryDbContext _breweryDbContext = breweryDbContext;
    async Task<Beer?> IAbstractRepository<Beer>.GetById(int id)
    {
        var beer = await _breweryDbContext.Beers.Include(b => b.Brewery).FirstOrDefaultAsync(beer => beer.Id == id);
        return beer;
    }
}