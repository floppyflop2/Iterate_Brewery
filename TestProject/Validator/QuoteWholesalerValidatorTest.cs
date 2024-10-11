using BusinessLayer.Validators;
using DataLayer.Interface;
using Domain;
using FluentValidation.TestHelper;
using Moq;

namespace UnitTests.Validator;

public class QuoteWholesalerValidatorTest
{
    private readonly Mock<IWholesalerRepository> _wholesalerRepositoryMock;
    private readonly Mock<IBeerRepository> _beerRepositoryMock;
    private readonly Wholesaler _wholesaler;
    private readonly QuoteValidator _validator;

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
            .WithErrorMessage("The Wholesaler cannot be empty");
    }

    [Fact]
    public async Task Should_Have_Error_When_OrderItems_Have_Duplicates()
    {
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new QuoteItem { BeerId = 1, Quantity = 10 },
                new QuoteItem { BeerId = 1, Quantity = 5 }
            }
        };
        var result = await _validator.TestValidateAsync(quote);
        result.ShouldHaveValidationErrorFor(q => q.OrderItems)
            .WithErrorMessage("There can't be any duplicate in the order");
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_OrderItems_Are_Unique()
    {
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new QuoteItem { BeerId = 1, Quantity = 10 },
                new QuoteItem { BeerId = 2, Quantity = 5 }
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
            .WithErrorMessage("The wholesaler must exist");
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