using BusinessLayer.Interface;
using DataLayer;
using DataLayer.Interface;
using Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages;

public partial class BeersOverview
{
    [Inject]
    public IBeerRepository? BeerRepository { get; set; }
    [Inject]
    public IQuoteService? QuoteService { get; set; }
    public List<Beer> Beers { get; set; } = new List<Beer>();

    public Quote? Quote { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var beers = await BeerRepository!.GetAll();
        Quote = new Quote
        {
            Wholesaler = null
        };
        Beers = beers.ToList()!;
    }

}