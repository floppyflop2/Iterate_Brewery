using DataLayer;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Brewery = Domain.Brewery;
namespace TestProject.UnitTests
{
    public class BreweryTest
    {
        private readonly BreweryRepository _repository;
        private readonly BreweryDbContext _dbContext;

        public BreweryTest()
        {
            var options = new DbContextOptionsBuilder<BreweryDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _dbContext = new BreweryDbContext(options);
            DbContextInitializer.Initialize(_dbContext);
            _repository = new BreweryRepository(_dbContext);
        }

        [Fact]
        public async Task AddBrewery_ShouldAddBrewery()
        {
            //Arrange
            var brewery = new Brewery { Id = 10, Name = "Test Brewery", CreatedDate = DateTime.Now };


            //Act 
            await _repository.Add(brewery);
            var result = _dbContext.Breweries.FirstOrDefault(b => brewery.Id == 10);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetBrewery_ShouldReturnBrewery()
        {

            //Arrange
            var brewery = _dbContext.Breweries.FirstOrDefault();

            //Act 
            var result = await _repository.GetById(brewery.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(brewery, result);
            Assert.Equal(brewery.Beers, result.Beers);
        }

        [Fact]
        public async Task DeleteBrewery_ShouldRemoveBreweryOnCascade()
        {
            //Arrange
            var brewery = _dbContext.Breweries.FirstOrDefault();
            var beers = _dbContext.Beers.Where(b => b.BreweryId == brewery.Id).ToList();

            //Act 
            await _repository.Remove(brewery);
            var result = await _repository.GetById(brewery.Id);

            //Assert
            Assert.Null(_dbContext.Breweries.FirstOrDefault(b => b.Id == brewery.Id));
            Assert.Empty(_dbContext.Beers.Where(b => b.BreweryId == brewery.Id));
            Assert.Null(result);
        }
    }
}