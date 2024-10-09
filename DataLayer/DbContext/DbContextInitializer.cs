using Domain;

namespace DataLayer.DbContext;
public static class DbContextInitializer
{
    private static List<Beer> _beers = new List<Beer>();
    private static List<Brewery> _breweries = new List<Brewery>();
    private static List<Wholesaler> _wholesalers = new List<Wholesaler>();


    public static void Initialize(BreweryDbContext context)
    {
        var random = new Random();
        context.Database.EnsureCreated();

        if (InitializeBreweries(context)) return;
        if (InitializeBeers(context, random)) return;
        if (InitializeWholesalers(context, random)) return;
    }

    private static bool InitializeBreweries(BreweryDbContext context)
    {
        if (context.Breweries.Any()) return true;

        _breweries = [
            new Brewery { Name = "Brewery 1" }, new Brewery { Name = "Brewery 2" }, new Brewery { Name = "Brewery 3" }
        ];
        foreach (var brewery in _breweries)
        {
            context.Breweries.Add(brewery);
        }
        context.SaveChanges();
        return false;
    }

    private static bool InitializeBeers(BreweryDbContext context, Random random)
    {
        if (context.Beers.Any()) return true;
        _beers =
        [
            new Beer { AlcoholContent = 6.0, Brewery = _breweries[random.Next(1, _breweries.Count)], Name = "Beer1", Price = 4.50},
            new Beer { AlcoholContent = 7.0, Brewery = _breweries[random.Next(1, _breweries.Count)], Name = "Beer2", Price = 5},
            new Beer { AlcoholContent = 5.0, Brewery = _breweries[random.Next(1, _breweries.Count)], Name = "Beer3", Price = 2},

        ];
        foreach (var beer in _beers)
        {
            context.Beers.Add(beer);
        }
        context.SaveChanges();
        return false;
    }

    private static bool InitializeWholesalers(BreweryDbContext context, Random random)
    {
        if (context.Wholesalers.Any()) return true;
        _wholesalers =
        [
            new Wholesaler
            {
                Name = "WholeSalers 1",
                Stocks = new List<WholesalerStock>()
            },

            new Wholesaler
            {
                Name = "WholeSalers 2",
                Stocks = new List<WholesalerStock>()
            },

            new Wholesaler
            {
                Name = "WholeSalers 3",
                Stocks = new List<WholesalerStock>()

            }
        ];
        foreach (var wholesaler in _wholesalers)
        {
            wholesaler.Stocks.Add(new WholesalerStock { Beer = GetRandomBeer(), Quantity = random.Next(1, 100), Wholesaler = wholesaler });
        }

        return false;
    }

    private static Beer GetRandomBeer()
    {
        return _beers[new Random().Next(0, _beers.Count)];
    }

    private static Brewery GetRandomBrewery()
    {
        return _breweries[new Random().Next(0, _breweries.Count)];
    }

    private static Wholesaler GetRandomWholesaler()
    {
        return _wholesalers[new Random().Next(0, _wholesalers.Count)];
    }
}
