namespace Domain;

public class Beer : BaseEntity
{
    public required string Name { get; set; }
    public double AlcoholContent { get; set; }
    public int BreweryId { get; set; }
    public required Brewery Brewery { get; set; }
    public double Price { get; set; }
}