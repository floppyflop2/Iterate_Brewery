using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants;
using DataLayer.Interface;
using Domain;

namespace BusinessLayer
{
    public class QuoteService
    {
        private readonly IWholesalerRepository _wholesalerRepository;
        private readonly IBeerRepository _beerRepository;
        private readonly IWholesalerStockRepository _wholesalerStockRepository;

        public QuoteService(IWholesalerRepository wholesalerRepository, IBeerRepository beerRepository, IWholesalerStockRepository wholesalerStockRepository)
        {
            _wholesalerRepository = wholesalerRepository;
            _beerRepository = beerRepository;
            _wholesalerStockRepository = wholesalerStockRepository;
        }

        public async Task<Quote> CreateQuote(List<QuoteItem> order, Wholesaler wholesaler)
        {
            var seller = await _wholesalerRepository.GetById(wholesaler.Id);
            if (seller == null)
                throw new ArgumentException(ErrorMessages.WholesalerMustExist);

            foreach (var item in order)
            {
                seller.Stocks.FirstOrDefault(stock => stock.BeerId == item.BeerId)!.Quantity -= item.Quantity;
            }
            await _wholesalerRepository.Update(seller);

            var quote = new Quote
            {
                Wholesaler = seller,
                OrderItems = order
            };

            quote.Price = CalculatePrice(order, wholesaler);

            return quote;
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


    }
}
