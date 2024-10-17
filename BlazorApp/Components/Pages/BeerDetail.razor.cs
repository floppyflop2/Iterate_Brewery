using DataLayer.Interface;
using Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages
{
    public partial class BeerDetail
    {

        [Inject]
        public IBeerRepository? BeerRepository { get; set; }
        [Parameter]
        public int BeerId { get; set; }

        public Beer Beer { get; set; } = new Beer();
    

        protected override async Task OnInitializedAsync()
        {
            var beer = await BeerRepository!.GetById(BeerId);
            Beer = beer;
        }
    }
}
