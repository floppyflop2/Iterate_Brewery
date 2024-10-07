using Brewery.Domain;

namespace Domain
{
    public class BeerStock
    {
        public Guid Id { get; set; }
        public required WholeSaler WholeSaler { get; set; }
        public required Beer Beer { get; set; }
        public int quantity {  get; set; }

    }
}