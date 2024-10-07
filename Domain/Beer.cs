using Domain;

namespace Brewery.Domain
{
    public class Beer
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public double AlcoholContent { get; set; }
        public double Price { get; set; }

        public required Brewer Brewer { get; set; }
    }
}
