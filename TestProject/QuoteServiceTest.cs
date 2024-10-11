using BusinessLayer;
using Constants;
using DataLayer.Interface;
using Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;

public class QuoteServiceTest
{
    private readonly Mock<IWholesalerRepository> _wholesalerRepositoryMock;
    private readonly QuoteService _quoteService;
    private readonly Wholesaler _wholesaler;
    public QuoteServiceTest()
    {
        _wholesalerRepositoryMock = new Mock<IWholesalerRepository>();
        _quoteService = new QuoteService(_wholesalerRepositoryMock.Object);
        _wholesaler = new Wholesaler
        {
            Id = 1,
            Name = "Test Wholesaler",
            Stocks = new List<WholesalerStock>
            {
                new WholesalerStock { BeerId = 1, Quantity = 20, Beer = new Beer { Id = 1, Name = "Beer1",Price = 5, Brewery = FakeDataFactory.Brewery}, Wholesaler = FakeDataFactory.GetFakeWholesaler()},
                new WholesalerStock { BeerId = 2, Quantity = 30, Beer = new Beer { Id = 2, Name = "Beer2",Price = 10, Brewery = FakeDataFactory.Brewery }, Wholesaler = FakeDataFactory.GetFakeWholesaler()}
            }
        };
    }

    [Fact]
    public async Task CreateQuote_Should_Throw_Exception_When_Wholesaler_Does_Not_Exist()
    {
        // Arrange
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Wholesaler)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _quoteService.CreateQuote(new List<QuoteItem>(), 1));
        Assert.Equal(ErrorMessages.WholesalerMustExist, exception.Message);
    }

    [Fact]
    public async Task CreateQuote_Should_Update_Wholesaler_Stock_Correctly()
    {
        // Arrange
        var wholesaler = _wholesaler;
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(wholesaler);

        var order = new List<QuoteItem>
        {
            new QuoteItem { BeerId = 1, Quantity = 5 },
            new QuoteItem { BeerId = 2, Quantity = 10 }
        };

        // Act
        var quote = await _quoteService.CreateQuote(order, 1);

        // Assert
        Assert.Equal(15, wholesaler.Stocks.First(s => s.BeerId == 1).Quantity);
        Assert.Equal(20, wholesaler.Stocks.First(s => s.BeerId == 2).Quantity);
        _wholesalerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Wholesaler>()), Times.Once);
    }

    [Fact]
    public async Task CreateQuote_Should_Calculate_Price_Correctly()
    {
        // Arrange
        var wholesaler = _wholesaler;
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(wholesaler);

        var order = new List<QuoteItem>
        {
            new QuoteItem { BeerId = 1, Quantity = 5 },
            new QuoteItem { BeerId = 2, Quantity = 10 }
        };

        // Act
        var quote = await _quoteService.CreateQuote(order, 1);

        // Assert
        Assert.Equal(112.5, quote.Price); // 5*5 + 10*10 = 112.5
    }

    [Fact]
    public async Task CreateQuote_Should_Apply_Discount_Correctly()
    {
        // Arrange
        var wholesaler = _wholesaler;
        _wholesalerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(wholesaler);

        var order = new List<QuoteItem>
        {
            new QuoteItem { BeerId = 1, Quantity = 15 },
            new QuoteItem { BeerId = 2, Quantity = 10 }
        };

        // Act
        var quote = await _quoteService.CreateQuote(order, 1);

        // Assert
        Assert.Equal(140, quote.Price); // (15*5 + 10*10) * 0.9 = 140
    }
}


