﻿using Domain;

namespace UnitTests;

public static class FakeDataFactory
{
    public static readonly Domain.Brewery Brewery = new()
    {
        Id = 1,
        Name = "Brewery 1"
    };

    public static readonly List<Beer> Beers =
    [
        new()
        {
            Id = 1,
            Name = "Beer 1",
            BreweryId = Brewery.Id,
            AlcoholContent = 5,
            Price = 10,
            Brewery = Brewery
        }
    ];

    public static Wholesaler GetFakeWholesaler()
    {
        var wholesaler = new Wholesaler
        {
            Id = 1,
            Name = "Wholesaler 1"
        };

        wholesaler.Stocks = new List<WholesalerStock>
        {
            new()
            {
                Id = 1,
                BeerId = 1,
                Quantity = 10,
                WholesalerId = wholesaler.Id,
                Wholesaler = wholesaler,
                Beer = Beers[0]
            },
            new()
            {
                Id = 2,
                BeerId = 2,
                Quantity = 20,
                WholesalerId = wholesaler.Id,
                Wholesaler = wholesaler,
                Beer = Beers[0]
            }
        };
        return wholesaler;
    }
}