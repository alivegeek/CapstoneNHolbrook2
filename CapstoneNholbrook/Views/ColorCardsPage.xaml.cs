using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.ViewModels;
using CapstoneNHolbrook.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace CapstoneNHolbrook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorCardsPage : ContentPage
    {
        // Add readonly fields for the services this page needs
        private readonly CRUDColorCards _crudColorCards;

        // Modify the constructor to accept the necessary services
        public ColorCardsPage(Client selectedClient, CRUDColorCards crudColorCards)
        {
            InitializeComponent();

            // Assign the service instances to the fields
            _crudColorCards = crudColorCards ?? throw new ArgumentNullException(nameof(crudColorCards));

            // Initialize the ViewModel with CRUDColorCards and set it as the BindingContext
            // Pass this.Navigation to provide the correct INavigation instance
            BindingContext = new ColorCardsPageViewModel(selectedClient, this.Navigation, _crudColorCards);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as ColorCardsPageViewModel;
            viewModel?.ReloadData();
        }
    }
}
