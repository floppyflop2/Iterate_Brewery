using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;

namespace DataLayer;

public class BeerRepository(BreweryDbContext breweryDbContext)
    : AbstractRepository<Beer>(breweryDbContext), IBeerRepository;