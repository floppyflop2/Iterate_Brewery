using Constants;
using DataLayer.Interface;
using Domain;
using FluentValidation;

namespace BusinessLayer.Validators;

public class QuoteValidator : AbstractValidator<Quote>
{
    public QuoteValidator(IWholesalerRepository wholesalerRepository, IBeerRepository beerRepository)
    {
        RuleFor(x => x.Wholesaler).NotNull().WithMessage(ErrorMessages.WholesalerCannotBeEmpty)
            .SetValidator(new QuoteWholesalerValidator(wholesalerRepository));

        RuleFor(quote => quote.OrderItems)
            .Must(order => order.GroupBy(o => o.BeerId).All(g => g.Count() == 1))
            .WithMessage(ErrorMessages.ThereCantBeAnyDuplicateInTheOrder);

        RuleForEach(x => x.OrderItems)
            .ChildRules(child =>
            {
                child.RuleForEach(item => new QuoteItemValidator(wholesalerRepository, beerRepository));
            });
    }
}