using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class WholesalerRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<Wholesaler>(breweryDbContext), IWholesalerRepository
{
    private readonly BreweryDbContext _breweryDbContext = breweryDbContext;

    async Task<Wholesaler?> IAbstractRepository<Wholesaler>.GetById(int id)
    {
        var wholesalers = await _breweryDbContext.Wholesalers
            .Include(b => b.Stocks).ThenInclude(s => s.Beer).FirstOrDefaultAsync(b => b.Id == id);
        return wholesalers;
    }
}