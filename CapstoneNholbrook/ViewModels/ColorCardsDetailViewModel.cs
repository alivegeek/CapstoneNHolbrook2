using System;
using System.Windows.Input;
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.ViewModels
{
    public class ColorCardsDetailViewModel : ViewModel
    {
        private readonly ColorCard _colorCard;
        private readonly CRUDColorCards _crudColorCards;
        private readonly ContentPage _page;

        public ColorCardsDetailViewModel(ColorCard colorCard, CRUDColorCards crudColorCards, ContentPage page)
        {
            _colorCard = colorCard ?? throw new ArgumentNullException(nameof(colorCard));
            // Debug/log to verify data
            System.Diagnostics.Debug.WriteLine($"ColorMixture: {colorCard.ColorMixture}, CreatedDate: {colorCard.CreatedDate}");
            _crudColorCards = crudColorCards ?? throw new ArgumentNullException(nameof(crudColorCards));
            _page = page ?? throw new ArgumentNullException(nameof(page));
        }

        // Exposing ColorCard properties to the view
        public string PreviousTint => _colorCard.PreviousTint;
        public string HairCondition => _colorCard.HairCondition;
        public string NaturalColor => _colorCard.NaturalColor;
        public int PercentGrey => _colorCard.PercentGrey;
        public string ColorMixture => _colorCard.ColorMixture;
        public DateTime CreatedDate => _colorCard.CreatedDate;

        // ICommand for deleting a color card.
        public ICommand DeleteColorCardCommand => new Command(async () => await DeleteColorCard());

        private async Task DeleteColorCard()
        {
            bool isUserAccept = await _page.DisplayAlert("Color Card Delete", "Are you sure you want to delete this color card?", "OK", "Cancel");
            if (isUserAccept)
            {
                await _crudColorCards.DeleteColorCardAsync(_colorCard.Id);  // Use the new async method
                await _page.Navigation.PopAsync(); // Go back to the previous page after deletion
            }
        }
    }
}
