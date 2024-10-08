using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;

namespace DataLayer;

public class WholesalerRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<Wholesaler>(breweryDbContext), IWholesalerRepository;