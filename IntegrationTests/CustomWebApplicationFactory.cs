using DataLayer.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var options = new DbContextOptionsBuilder<BreweryDbContext>()
            .UseInMemoryDatabase("InMemoryDatabase")
            .Options;

        var dbContext = new BreweryDbContext(options);
        DbContextInitializer.Initialize(dbContext);

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(options);
            services.AddSingleton(dbContext);
        });
        builder.UseEnvironment("Development");
    }
}