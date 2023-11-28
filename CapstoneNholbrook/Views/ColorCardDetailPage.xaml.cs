using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace CapstoneNHolbrook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorCardDetailPage : ContentPage
    {
        public ColorCardDetailPage(ColorCard selectedColorCard, CRUDColorCards crudColorCards)
        {
            InitializeComponent();

            // Ensure that Navigation is available in your ViewModel
            BindingContext = new ColorCardsDetailViewModel(selectedColorCard, crudColorCards, this);
        }
    }
}
