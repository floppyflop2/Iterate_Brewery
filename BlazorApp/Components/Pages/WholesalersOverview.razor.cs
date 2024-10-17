using DataLayer.Interface;
using Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages
{
    public partial class WholesalersOverview
    {
        public List<Wholesaler>? Wholesalers { get; set; } = new List<Wholesaler>();
        [Inject]
        public IWholesalerRepository? WholesalerRepository { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Wholesalers = await WholesalerRepository!.GetAll() as List<Wholesaler> ?? [];
        }
    }
}
