using BusinessLayer.Validators;
using DataLayer.Interface;
using Domain;
using FluentValidation.TestHelper;
using Moq;

namespace UnitTests.Validator;

public class QuoteValidatorTest
{
    private readonly QuoteValidator _validator;
    private readonly Mock<IWholesalerRepository> _wholesalerRepositoryMock;
    private readonly Mock<IBeerRepository> _beerRepositoryMock;
    private readonly Wholesaler _wholesaler;

    public QuoteValidatorTest()
    {
        _wholesalerRepositoryMock = new Mock<IWholesalerRepository>();
        _beerRepositoryMock = new Mock<IBeerRepository>();
        _wholesaler = FakeDataFactory.GetFakeWholesaler();
        _validator = new QuoteValidator(_wholesalerRepositoryMock.Object, _beerRepositoryMock.Object, _wholesaler);
    }

    [Fact]
    public async Task Should_Have_Error_When_Saler_Id_Is_Invalid()
    {
        // Arrange
        var quote = new Quote { Wholesaler = new Wholesaler { Id = 0, Name = "Name" } };

        // Act
        var result = await _validator.TestValidateAsync(quote);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Wholesaler.Id);
    }

    [Fact]
    public async Task Should_Have_Error_When_OrderItems_Have_Duplicates()
    {
        // Arrange
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new QuoteItem { BeerId = 1, Quantity = 10 },
                new QuoteItem { BeerId = 1, Quantity = 5 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(quote);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.OrderItems)
            .WithErrorMessage("There can't be any duplicate in the order");
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_OrderItems_Are_Unique()
    {
        // Arrange
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new QuoteItem { BeerId = 1, Quantity = 10 },
                new QuoteItem { BeerId = 2, Quantity = 5 }
            }
        };

        // Act
        var result = await _validator.TestValidateAsync(quote);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.OrderItems);
    }

    [Fact]
    public async Task Should_Validate_Each_OrderItem_With_QuoteItemValidator()
    {
        // Arrange
        var quote = new Quote
        {
            Wholesaler = _wholesaler,
            OrderItems = new List<QuoteItem>
            {
                new QuoteItem { BeerId = 1, Quantity = 10 },
                new QuoteItem { BeerId = 2, Quantity = 5 }
            }
        };
        _beerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(FakeDataFactory.Beers.First);
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);
        // Act
        var result = await _validator.TestValidateAsync(quote);

        // Assert
        foreach (var item in quote.OrderItems)
        {
            var itemValidator = new QuoteItemValidator(_wholesalerRepositoryMock.Object, _beerRepositoryMock.Object, item.BeerId, _wholesaler);
            var itemResult = await itemValidator.TestValidateAsync(item);
            itemResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}