using System;
using System.Windows.Input;
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.ViewModels
{
    public class AddColorCardPageViewModel : ViewModel
    {
        private readonly Client _client;
        private readonly CRUDColorCards _crudColorCards;
        private readonly INavigation _navigation;

        // Properties for each field of the ColorCard...
        public string PreviousTint { get; set; }
        public string HairCondition { get; set; }
        public string NaturalColor { get; set; }
        public int PercentGrey { get; set; }
        public string Texture { get; set; }
        public string LightenerMixture { get; set; }
        public string ColorMixture { get; set; }

        public ICommand AddColorCardCommand { get; }

        public AddColorCardPageViewModel(Client client, CRUDColorCards crudColorCards, INavigation navigation)
        {
            _client = client;
            _crudColorCards = crudColorCards;
            _navigation = navigation;

            AddColorCardCommand = new Command(async () => await AddColorCardAsync());
        }

        private async Task AddColorCardAsync()
        {
            var newColorCard = new ColorCard
            {
                PreviousTint = PreviousTint,
                HairCondition = HairCondition,
                NaturalColor = NaturalColor,
                PercentGrey = PercentGrey,
                Texture = Texture,
                LightenerMixture = LightenerMixture,
                ColorMixture = ColorMixture,
                ClientId = _client.Id,
                CreatedDate = DateTime.Now
            };

            await _crudColorCards.AddColorCardAsync(newColorCard);

            // Optionally navigate away
            await _navigation.PopAsync();
        }
    }
}
