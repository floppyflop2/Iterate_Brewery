using System.Text.Json.Serialization;
using BusinessLayer;
using BusinessLayer.Interface;
using BusinessLayer.Validators;
using DataLayer;
using DataLayer.DbContext;
using DataLayer.Interface;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BreweryDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddScoped(typeof(IAbstractRepository<>), typeof(AbstractRepository<>));
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
builder.Services.AddScoped<IBreweryRepository, BreweryRepository>();
builder.Services.AddScoped<IWholesalerRepository, WholesalerRepository>();
builder.Services.AddScoped<IWholesalerStockRepository, WholesalerStockRepository>();

builder.Services.AddScoped<IValidator<Quote>, QuoteValidator>();
builder.Services.AddScoped<IValidator<QuoteItem>, QuoteItemValidator>();
builder.Services.AddScoped<IValidator<Wholesaler>, QuoteWholesalerValidator>();
builder.Services.AddScoped<IValidator<Beer>, BeerValidator>();

builder.Services.AddScoped<IQuoteService, QuoteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BreweryDbContext>();
    DbContextInitializer.Initialize(context);
}

app.Run();

//This has to be added to be referenced in the test project for the factory
namespace Brewery
{
    public class Program
    {
    }
}