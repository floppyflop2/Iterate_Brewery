using DataLayer.Interface;
using Domain;
using FluentValidation;

namespace BusinessLayer.Validators;
public class QuoteValidator : AbstractValidator<Quote>
{
    public QuoteValidator(IWholesalerRepository wholesalerRepository, IBeerRepository beerRepository, Wholesaler wholesaler)
    {

        RuleFor(x => x.Saler)
            .SetValidator(new QuoteWholesalerValidator(wholesalerRepository));

        RuleFor(quote => quote.OrderItems)
            .Must(order => order.GroupBy(o => o.BeerId).All(g => g.Count() == 1))
            .WithMessage("There can't be any duplicate in the order");

        RuleForEach(x => x.OrderItems)
            .ChildRules(child =>
            {
                child.RuleForEach(item => new QuoteItemValidator(wholesalerRepository, beerRepository, item.BeerId, wholesaler));
            });
    }

    private static double CalculatePrice(List<QuoteItem> order, Wholesaler wholesaler)
    {
        double total = 0;
        int totalQuantity = 0;

        foreach (var item in order)
        {
            var beer = wholesaler.Stocks.First(s => s.BeerId == item.BeerId).Beer;
            total += item.Quantity * beer.Price;
            totalQuantity += item.Quantity;
        }

        if (totalQuantity > 20)
            total *= 0.8;
        else if (totalQuantity > 10)
            total *= 0.9;

        return total;
    }

    private static string GenerateSummary(List<QuoteItem> order, Wholesaler wholesaler)
    {
        var summary = "";
        summary = $"Order Summary:\n";
        var price = CalculatePrice(order, wholesaler);
        foreach (var item in order)
        {
            var beer = wholesaler.Stocks.First(s => s.BeerId == item.BeerId).Beer;
            summary += $"{item.Quantity} x {beer.Name} @ {beer.Price:C} each\n";
        }
        summary += $"Total Price: {price:C}";
        return summary;
    }
}

