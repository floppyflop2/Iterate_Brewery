namespace Brewery.Domain
{
    public class Beer
    {
        public required string Name { get; set; }

        public double AlcoholContent { get; set; }

        public double Price { get; set; }
    }
}
