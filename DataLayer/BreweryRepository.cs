using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class BreweryRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<Brewery>(breweryDbContext), IBreweryRepository
{
    private readonly BreweryDbContext _breweryDbContext = breweryDbContext;

    async Task<Brewery?> IAbstractRepository<Brewery>.GetById(int id)
    {
        var brewery = await _breweryDbContext.Breweries.Include(b => b.Beers).FirstOrDefaultAsync(b => b.Id == id);
        return brewery;
    }
}
