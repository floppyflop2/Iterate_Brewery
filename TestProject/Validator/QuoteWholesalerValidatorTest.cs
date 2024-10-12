using BusinessLayer.Validators;
using Constants;
using DataLayer.Interface;
using Domain;
using FluentValidation.TestHelper;
using Moq;

namespace UnitTests.Validator;

public class QuoteWholesalerValidatorTest
{
    private readonly Mock<IBeerRepository> _beerRepositoryMock;
    private readonly QuoteValidator _validator;
    private readonly Wholesaler _wholesaler;
    private readonly Mock<IWholesalerRepository> _wholesalerRepositoryMock;

    public QuoteWholesalerValidatorTest()
    {
        _wholesalerRepositoryMock = new Mock<IWholesalerRepository>();
        _beerRepositoryMock = new Mock<IBeerRepository>();
        _wholesaler = new Wholesaler { Id = 1, Name = "Test Wholesaler" };

        _validator = new QuoteValidator(_wholesalerRepositoryMock.Object, _beerRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Wholesaler_Is_Null()
    {
        var quote = new Quote { Wholesaler = null };
        var result = await _validator.TestValidateAsync(quote);
        result.ShouldHaveValidationErrorFor(q => q.Wholesaler)
            .WithErrorMessage(ErrorMessages.WholesalerCannotBeEmpty);
    }

    [Fact]
    public async Task Should_Have_Error_When_OrderItems_Have_Duplicates()
    {
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new() { BeerId = 1, Quantity = 10 },
                new() { BeerId = 1, Quantity = 5 }
            }
        };
        var result = await _validator.TestValidateAsync(quote);
        result.ShouldHaveValidationErrorFor(q => q.OrderItems)
            .WithErrorMessage(ErrorMessages.ThereCantBeAnyDuplicateInTheOrder);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_OrderItems_Are_Unique()
    {
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new() { BeerId = 1, Quantity = 10 },
                new() { BeerId = 2, Quantity = 5 }
            }
        };
        var result = await _validator.TestValidateAsync(quote);
        result.ShouldNotHaveValidationErrorFor(q => q.OrderItems);
    }

    [Fact]
    public async Task Should_Have_Error_When_Wholesaler_Does_Not_Exist()
    {
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Wholesaler)null);

        var quote = new Quote { Wholesaler = new Wholesaler { Id = 999, Name = "Name" } };
        var validator = new QuoteWholesalerValidator(_wholesalerRepositoryMock.Object);
        var result = await validator.TestValidateAsync(quote.Wholesaler);

        result.ShouldHaveValidationErrorFor(w => w.Id)
            .WithErrorMessage(ErrorMessages.WholesalerMustExist);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Wholesaler_Exists()
    {
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(_wholesaler);

        var quote = new Quote { Wholesaler = _wholesaler };
        var validator = new QuoteWholesalerValidator(_wholesalerRepositoryMock.Object);
        var result = await validator.TestValidateAsync(quote.Wholesaler);

        result.ShouldNotHaveValidationErrorFor(w => w.Id);
    }
}