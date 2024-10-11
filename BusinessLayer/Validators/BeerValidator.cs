using DataLayer;
using DataLayer.Interface;
using Domain;
using FluentValidation;

namespace BusinessLayer.Validators;

public class BeerValidator : AbstractValidator<Beer>
{
    public BeerValidator(IBeerRepository beerRepository)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Beer Name is mandatory")
            .MustAsync(async (name, cancellation) =>
        {
            var beer = await beerRepository.FirstOrDefault(b => b!.Name == name);
            return beer == null;
        }).WithMessage("This name is already in use");

        RuleFor(x => x.AlcoholContent).InclusiveBetween(0, 100);
        RuleFor(x => x.Price).InclusiveBetween(0, 60);
    }
}