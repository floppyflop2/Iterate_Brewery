using Domain;

namespace BusinessLayer.Interface;

public interface IQuoteService
{
    public Task<Quote> CreateQuote(List<QuoteItem> order, int wholesalerId);
}