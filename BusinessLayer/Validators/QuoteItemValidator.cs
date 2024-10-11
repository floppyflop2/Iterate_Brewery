using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Interface;
using Domain;
using FluentValidation;

namespace BusinessLayer.Validators
{
    public class QuoteItemValidator : AbstractValidator<QuoteItem>
    {
        public QuoteItemValidator(IWholesalerRepository wholesalerRepository, IBeerRepository beerRepository)
        {
            RuleFor(x => x.BeerId).MustAsync(async (id, cancellation) =>
            {
                var beer = await beerRepository.GetById(id);
                return beer != null;
            }).WithMessage("The beer must exist");

            RuleFor(x => x.Quantity)
                .Must(x => x > 0).WithMessage("The quantity must be greater than 0");

            //Wholesaler Validity has been checked in QuoteValidator by the WholesalerValidator
            RuleFor(x => x).MustAsync(async (quoteItem, cancellation) =>
            {
                var seller = await wholesalerRepository.GetById(quoteItem.WholeSalerId);
                var stock = seller!.Stocks.FirstOrDefault(stock => stock.BeerId == quoteItem.BeerId);
                return stock != null;
            }).WithMessage("The beer must be sold by the wholesaler")
                .DependentRules(() =>
                {
                    RuleFor(x => x).MustAsync(async (quoteItem, cancellation) =>
                    {
                        var seller = await wholesalerRepository.GetById(quoteItem.WholeSalerId);
                        var stock = seller!.Stocks.FirstOrDefault(stock => stock.BeerId == quoteItem.BeerId);
                        return stock!.Quantity >= quoteItem.Quantity;
                    }).WithMessage("The quantity must be less than or equal to the stock");
                });



        }
    }
}
