using System.Linq.Expressions;
using BusinessLayer.Validators;
using DataLayer.Interface;
using Domain;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace UnitTests.Validator
{
    public class QuoteItemValidatorTest
    {
        private readonly QuoteItemValidator _validator;
        private readonly Mock<IWholesalerRepository> _wholesalerRepositoryMock;
        private readonly Mock<IBeerRepository> _beerRepositoryMock;
        private readonly Wholesaler _wholesaler;
        private readonly int _beerId = 1;
        private readonly Beer _testBeer;

        public QuoteItemValidatorTest()
        {
            _wholesalerRepositoryMock = new Mock<IWholesalerRepository>();
            _beerRepositoryMock = new Mock<IBeerRepository>();
            _wholesaler = new Wholesaler
            {
                Id = 1,
                Name = "Name",
            };

            _testBeer = new Beer
            {
                Id = 1,
                Name = "Test Beer",
                Brewery = new Domain.Brewery { Id = 1, Name = "Test Brewery" },
            };

            var stock =
                new WholesalerStock
                {
                    BeerId = _beerId,
                    Quantity = 100,
                    Wholesaler = _wholesaler,
                    Beer = _testBeer
                };
            _wholesaler.Stocks.Add(stock);

            _validator = new QuoteItemValidator(_wholesalerRepositoryMock.Object, _beerRepositoryMock.Object, _beerId, _wholesaler);
        }

        [Fact]
        public async Task Should_Have_Error_When_BeerId_Does_Not_Exist()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10 };
            _beerRepositoryMock.Setup(repo => repo.GetById(_beerId)).ReturnsAsync((Beer)null!);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.BeerId)
                .WithErrorMessage("The beer must exist");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_BeerId_Exists()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10 };
            _beerRepositoryMock.Setup(repo => repo.GetById(_beerId)).ReturnsAsync(new Beer());

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q.BeerId);
        }

        [Fact]
        public async Task Should_Have_Error_When_Quantity_Is_Less_Than_Or_Equal_To_Zero()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 0 };

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Quantity)
                .WithErrorMessage("The quantity must be greater than 0");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Quantity_Is_Greater_Than_Zero()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10 };

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q.Quantity);
        }

        [Fact]
        public async Task Should_Have_Error_When_Wholesaler_Does_Not_Sell_The_Beer()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10 };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(new Wholesaler
            {
                Id = _wholesaler.Id,
                Stocks = new List<WholesalerStock>()
            });

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q)
                .WithErrorMessage("The beer must be sold by the wholesaler");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Wholesaler_Sells_The_Beer()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10 };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q);
        }

        [Fact]
        public async Task Should_Have_Error_When_Quantity_Exceeds_Stock()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 200 };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q)
                .WithErrorMessage("The quantity must be less than or equal to the stock");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Quantity_Is_Less_Than_Or_Equal_To_Stock()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 50 };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q);
        }
    }
}