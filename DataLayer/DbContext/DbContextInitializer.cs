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

        if (context.Breweries.Any())
        {
            return;
        }

        _breweries = [
            new Brewery { Name = "Brewery 1" }, new Brewery { Name = "Brewery 2" }, new Brewery { Name = "Brewery 3" }
        ];

        _beers =
        [
            new Beer { AlcoholContent = 6.0, Brewery = _breweries[random.Next(1, _breweries.Count)], Name = "Beer1" },
            new Beer { AlcoholContent = 7.0, Brewery = _breweries[random.Next(1, _breweries.Count)], Name = "Beer2" },
            new Beer { AlcoholContent = 5.0, Brewery = _breweries[random.Next(1, _breweries.Count)], Name = "Beer3" },

        ];

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

        foreach (var beer in _beers)
        {
            context.Beers.Add(beer);
        }

        foreach (var brewery in _breweries)
        {
            context.Breweries.Add(brewery);
        }

        foreach (var wholesaler in _wholesalers)
        {
            wholesaler.Stocks.Add(new WholesalerStock { Beer = GetRandomBeer(), Quantity = random.Next(1, 100), Wholesaler = wholesaler });
        }

        context.SaveChanges();
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
