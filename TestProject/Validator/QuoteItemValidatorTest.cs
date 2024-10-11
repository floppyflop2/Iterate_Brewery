using System.Linq.Expressions;
using BusinessLayer.Validators;
using Constants;
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
        private readonly int _beerId;
        private readonly Beer _testBeer;

        public QuoteItemValidatorTest()
        {
            _wholesalerRepositoryMock = new Mock<IWholesalerRepository>();
            _beerRepositoryMock = new Mock<IBeerRepository>();
            _testBeer = FakeDataFactory.Beers[0];
            _beerId = _testBeer.Id;
            _wholesaler = FakeDataFactory.GetFakeWholesaler();
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);
            _validator = new QuoteItemValidator(_wholesalerRepositoryMock.Object, _beerRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Have_Error_When_BeerId_Does_Not_Exist()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10, WholeSalerId = _wholesaler.Id};
            _beerRepositoryMock.Setup(repo => repo.GetById(_beerId)).ReturnsAsync((Beer)null!);
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);
            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.BeerId)
                .WithErrorMessage(ErrorMessages.BeerMustExist);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_BeerId_Exists()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10, WholeSalerId = _wholesaler.Id };
            _beerRepositoryMock.Setup(repo => repo.GetById(_beerId)).ReturnsAsync(_testBeer);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q.BeerId);
        }

        [Fact]
        public async Task Should_Have_Error_When_Quantity_Is_Less_Than_Or_Equal_To_Zero()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 0, WholeSalerId = _wholesaler.Id };

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q.Quantity)
                .WithErrorMessage(ErrorMessages.QuantityMustBeGreaterThanZero);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Quantity_Is_Greater_Than_Zero()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10, WholeSalerId = _wholesaler.Id };

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q.Quantity);
        }

        [Fact]
        public async Task Should_Have_Error_When_Wholesaler_Does_Not_Sell_The_Beer()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10, WholeSalerId = _wholesaler.Id };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(new Wholesaler
            {
                Id = _wholesaler.Id,
                Stocks = new List<WholesalerStock>(),
                Name = "Name"
            });

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q)
                .WithErrorMessage(ErrorMessages.BeerMustBeSoldByWholesaler);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Wholesaler_Sells_The_Beer()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10, WholeSalerId = _wholesaler.Id };
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
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 200, WholeSalerId = _wholesaler.Id };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldHaveValidationErrorFor(q => q)
                .WithErrorMessage(ErrorMessages.QuantityMustBeLessThanOrEqualToStock);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Quantity_Is_Less_Than_Or_Equal_To_Stock()
        {
            // Arrange
            var quoteItem = new QuoteItem { BeerId = _beerId, Quantity = 10, WholeSalerId = _wholesaler.Id };
            _wholesalerRepositoryMock.Setup(repo => repo.GetById(_wholesaler.Id)).ReturnsAsync(_wholesaler);

            // Act
            var result = await _validator.TestValidateAsync(quoteItem);

            // Assert
            result.ShouldNotHaveValidationErrorFor(q => q);
        }
    }
}