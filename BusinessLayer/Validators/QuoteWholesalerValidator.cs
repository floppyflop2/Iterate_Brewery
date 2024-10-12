using Constants;
using DataLayer.Interface;
using Domain;
using FluentValidation;

namespace BusinessLayer.Validators;

//This class is used to validate the wholesaler of a quote
public class QuoteWholesalerValidator : AbstractValidator<Wholesaler>
{
    public QuoteWholesalerValidator(IWholesalerRepository wholesalerRepository)
    {
        RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
        {
            var wholesaler = await wholesalerRepository.GetById(id);
            return wholesaler != null;
        }).WithMessage(ErrorMessages.WholesalerMustExist);
    }
}