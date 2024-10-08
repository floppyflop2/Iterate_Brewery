using DataLayer;
using DataLayer.DbContext;
using DataLayer.Interface;
using System;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BreweryDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddTransient(typeof(IAbstractRepository<>), typeof(AbstractRepository<>));
builder.Services.AddTransient<IBeerRepository, BeerRepository>();
builder.Services.AddTransient<IBreweryRepository, BreweryRepository>();
builder.Services.AddTransient<IWholesalerRepository, WholesalerRepository>();
builder.Services.AddTransient<IWholesalerStockRepository, WholesalerStockRepository>();

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
