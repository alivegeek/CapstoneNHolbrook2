using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.ViewModels;
using Microsoft.Maui.Controls; // MAUI equivalent of Xamarin.Forms

namespace CapstoneNHolbrook.Views
{
    public partial class AddAppointment : ContentPage
    {
        public AddAppointment(CRUDAppointments crudAppointments, CRUDClients crudClients, Client selectedClient = null)
        {
            InitializeComponent();

            BindingContext = new AddAppointmentViewModel(crudAppointments, crudClients, selectedClient);
        }

        private AddAppointmentViewModel ViewModel => BindingContext as AddAppointmentViewModel;

        private void OnClientSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel?.FilterClients(e.NewTextValue);
        }

        // Updated for .NET MAUI to use CollectionView
        private void OnClientSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedClient = e.CurrentSelection.FirstOrDefault() as Client;
            ViewModel.SelectedClient = selectedClient;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.SelectedClient != null)
            {
                // In MAUI, you might use ScrollTo to achieve a similar effect
                ClientListView.ScrollTo(ViewModel.SelectedClient, animate: true);
            }
        }

    }
}
