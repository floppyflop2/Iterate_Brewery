using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;

namespace DataLayer;

public class BreweryRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<Brewery>(breweryDbContext), IBreweryRepository;