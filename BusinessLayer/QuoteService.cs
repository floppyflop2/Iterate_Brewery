using BusinessLayer.Interface;
using Constants;
using DataLayer.Interface;
using Domain;

namespace BusinessLayer;

public class QuoteService(
    IWholesalerRepository wholesalerRepository)
    : IQuoteService
{
    private readonly IWholesalerRepository _wholesalerRepository = wholesalerRepository;

    public async Task<Quote> CreateQuote(List<QuoteItem> order, int wholesalerId)
    {
        var wholesaler = await wholesalerRepository.GetById(wholesalerId);
        if (wholesaler == null)
            throw new ArgumentException(ErrorMessages.WholesalerMustExist);

        foreach (var item in order)
            wholesaler.Stocks.FirstOrDefault(stock => stock.BeerId == item.BeerId)!.Quantity -= item.Quantity;
        await _wholesalerRepository.Update(wholesaler);

        var quote = new Quote
        {
            Wholesaler = wholesaler,
            OrderItems = order,
            Price = CalculatePrice(order, wholesaler)
        };

        return quote;
    }

    private static double CalculatePrice(List<QuoteItem> order, Wholesaler wholesaler)
    {
        double total = 0;
        var totalQuantity = 0;

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
}