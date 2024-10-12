namespace Domain;

public class Wholesaler : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<WholesalerStock> Stocks { get; set; } = new List<WholesalerStock>();
}