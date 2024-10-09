using DataLayer.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Brewery;

namespace IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var options = new DbContextOptionsBuilder<BreweryDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        var dbContext = new BreweryDbContext(options);
        DbContextInitializer.Initialize(dbContext);

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<DbContextOptions<BreweryDbContext>>(options);
            services.AddSingleton<BreweryDbContext>(dbContext);
        });
        builder.UseEnvironment("Development");
    }
}