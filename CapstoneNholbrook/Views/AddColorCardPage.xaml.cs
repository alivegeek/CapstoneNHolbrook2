using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.ViewModels;

namespace CapstoneNHolbrook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddColorCardPage : ContentPage
    {
        public AddColorCardPage(Client client, CRUDColorCards crudColorCards)
        {
            InitializeComponent();

            // Initialize ViewModel and set as BindingContext
            BindingContext = new AddColorCardPageViewModel(client, crudColorCards, Navigation);
        }
    }
}
