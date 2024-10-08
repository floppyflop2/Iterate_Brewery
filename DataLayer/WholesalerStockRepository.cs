using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;

namespace DataLayer;

public class WholesalerStockRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<WholesalerStock>(breweryDbContext), IWholesalerStockRepository;