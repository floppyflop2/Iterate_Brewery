﻿namespace Domain;

public class Brewery : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<Beer> Beers { get; set; } = new List<Beer>();
}