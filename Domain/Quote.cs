namespace Domain;

public class Quote
{
    public double Price { get; set; }
    public required Wholesaler Wholesaler { get; set; }
    public List<QuoteItem> OrderItems { get; set; } = new();
}