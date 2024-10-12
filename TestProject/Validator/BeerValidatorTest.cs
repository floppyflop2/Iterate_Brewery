using System.Linq.Expressions;
using BusinessLayer.Validators;
using Constants;
using DataLayer.Interface;
using Domain;
using FluentValidation.TestHelper;
using Moq;

namespace UnitTests.Validator;

public class BeerValidatorTest
{
    private readonly Mock<IBeerRepository> _beerRepositoryMock;
    private readonly Domain.Brewery _brewery;
    private readonly BeerValidator _validator;

    public BeerValidatorTest()
    {
        _beerRepositoryMock = new Mock<IBeerRepository>();
        _brewery = new Domain.Brewery { Name = "Test Brewery" };
        _validator = new BeerValidator(_beerRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Null()
    {
        // Arrange
        var beer = new Beer { Name = null, Brewery = _brewery };

        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Name)
            .WithErrorMessage(ErrorMessages.BeerNameIsMandatory);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        // Arrange
        var beer = new Beer
        {
            Name = "Test Beer",
            Brewery = _brewery
        };
        _beerRepositoryMock
            .Setup(repo => repo.FirstOrDefault(It.IsAny<Expression<Func<Beer?, bool>>>()))
            .ReturnsAsync(beer);
        var beerResult = _beerRepositoryMock.Object.FirstOrDefault(b => b!.Name == beer.Name);
        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Name)
            .WithErrorMessage(ErrorMessages.ThisNameIsAlreadyInUse);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Name_Is_Unique()
    {
        // Arrange
        var beer = new Beer
        {
            Name = "Unique Beer",
            Brewery = _brewery
        };
        _beerRepositoryMock.Setup(repo => repo.FirstOrDefault(It.IsAny<Expression<Func<Beer?, bool>>>()))
            .ReturnsAsync((Beer)null!);

        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldNotHaveValidationErrorFor(b => b.Name);
    }

    [Fact]
    public async Task Should_Have_Error_When_AlcoholContent_Is_Out_Of_Range()
    {
        // Arrange
        var beer = new Beer
        {
            AlcoholContent = 150,
            Name = "Name",
            Brewery = _brewery
        };

        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.AlcoholContent);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_AlcoholContent_Is_In_Range()
    {
        // Arrange
        var beer = new Beer
        {
            AlcoholContent = 50,
            Name = "Name",
            Brewery = _brewery
        };

        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldNotHaveValidationErrorFor(b => b.AlcoholContent);
    }

    [Fact]
    public async Task Should_Have_Error_When_Price_Is_Out_Of_Range()
    {
        // Arrange
        var beer = new Beer
        {
            Price = 100,
            Name = "Name",
            Brewery = _brewery
        };

        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldHaveValidationErrorFor(b => b.Price);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Price_Is_In_Range()
    {
        // Arrange
        var beer = new Beer
        {
            Price = 30,
            Name = "Name",
            Brewery = _brewery
        };

        // Act
        var result = await _validator.TestValidateAsync(beer);

        // Assert
        result.ShouldNotHaveValidationErrorFor(b => b.Price);
    }
}