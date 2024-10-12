using Constants;
using DataLayer.Interface;
using Domain;
using FluentValidation;

namespace BusinessLayer.Validators;

public class QuoteItemValidator : AbstractValidator<QuoteItem>
{
    public QuoteItemValidator(IWholesalerRepository wholesalerRepository, IBeerRepository beerRepository)
    {
        RuleFor(x => x.BeerId).MustAsync(async (id, cancellation) =>
        {
            var beer = await beerRepository.GetById(id);
            return beer != null;
        }).WithMessage(ErrorMessages.BeerMustExist);

        RuleFor(x => x.Quantity)
            .Must(x => x > 0).WithMessage(ErrorMessages.QuantityMustBeGreaterThanZero);

        //Wholesaler Validity has been checked in QuoteValidator by the WholesalerValidator
        RuleFor(x => x).MustAsync(async (quoteItem, cancellation) =>
            {
                var seller = await wholesalerRepository.GetById(quoteItem.WholeSalerId);
                var stock = seller!.Stocks.FirstOrDefault(stock => stock.BeerId == quoteItem.BeerId);
                return stock != null;
            }).WithMessage(ErrorMessages.BeerMustBeSoldByWholesaler)
            .DependentRules(() =>
            {
                RuleFor(x => x).MustAsync(async (quoteItem, cancellation) =>
                {
                    var seller = await wholesalerRepository.GetById(quoteItem.WholeSalerId);
                    var stock = seller!.Stocks.FirstOrDefault(stock => stock.BeerId == quoteItem.BeerId);
                    return stock!.Quantity >= quoteItem.Quantity;
                }).WithMessage(ErrorMessages.QuantityMustBeLessThanOrEqualToStock);
            });
    }
}