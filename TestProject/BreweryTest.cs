using DataLayer;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class BreweryTest
{
    [Fact]
    public async Task AddBrewery_ShouldAddBrewery()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<BreweryDbContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(nameof(AddBrewery_ShouldAddBrewery))
            .Options;
        await using var dbContext = new BreweryDbContext(options);
        DbContextInitializer.Initialize(dbContext);
        var repository = new BreweryRepository(dbContext);
        var brewery = new Domain.Brewery { Id = 10, Name = "Test Brewery", CreatedDate = DateTime.Now };

        //Act 
        await repository.Add(brewery);
        Domain.Brewery? result;
        result = await dbContext.Breweries.FirstOrDefaultAsync(b => brewery.Id == 10);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetBrewery_ShouldReturnBrewery()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<BreweryDbContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(nameof(GetBrewery_ShouldReturnBrewery))
            .Options;
        await using var dbContext = new BreweryDbContext(options);
        DbContextInitializer.Initialize(dbContext);
        var repository = new BreweryRepository(dbContext);
        var breweries = await repository.GetAll();
        var entity = breweries.FirstOrDefault();
        //Act 
        var result = await repository.GetById(entity!.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result, result);
        Assert.Equal(result.Beers, result.Beers);
    }

    [Fact]
    public async Task DeleteBrewery_ShouldRemoveBreweryOnCascade()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<BreweryDbContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(nameof(DeleteBrewery_ShouldRemoveBreweryOnCascade))
            .Options;
        await using var dbContext = new BreweryDbContext(options);
        DbContextInitializer.Initialize(dbContext);
        var repository = new BreweryRepository(dbContext);
        var breweries = await repository.GetAll();
        var entity = breweries.FirstOrDefault();

        //Act 
        await repository.Remove(entity);
        var result = await repository.GetById(entity!.Id);
        //Assert
        var containsBeer = await dbContext.Beers.AnyAsync(b => b.BreweryId == entity.Id);
        Assert.False(containsBeer);
        Assert.Null(result);
    }
}