using System.Text.Json.Serialization;
using BlazorApp.Components;
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

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();

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
builder.Services.AddRazorComponents().AddInteractiveServerComponents();


var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BreweryDbContext>();
    DbContextInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
