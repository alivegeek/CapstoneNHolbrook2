using CapstoneNHolbrook.ViewModels;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.Models;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace CapstoneNHolbrook.Views
{
    public partial class ClientPage : ContentPage
    {
        private Client selectedClient;
        private CRUDAppointments crudAppointments;
        private CRUDClients crudClients;
        private CRUDColorCards crudColorCards;

        // Updated constructor with CRUDColorCards as a parameter
        public ClientPage(Client selectedClient, CRUDAppointments crudAppointments, CRUDClients crudClients, CRUDColorCards crudColorCards)
        {
            InitializeComponent();

            this.selectedClient = selectedClient;
            this.crudAppointments = crudAppointments;
            this.crudClients = crudClients;
            this.crudColorCards = crudColorCards; // Set from constructor parameter

            // Asynchronously get the client data from the database
            Task.Run(async () =>
            {
                Client clientData = await crudClients.GetClientAsync(selectedClient.Id);

                // Set the BindingContext
                BindingContext = new ClientPageViewModel(clientData, this.Navigation, crudAppointments, crudClients, crudColorCards);
            }).Wait(); // Be cautious with Wait(), consider refactoring to async
        }

        // Edit Client Button Click Event Handler
        private async void EditClientButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to the AddClient page with the necessary parameters
            await this.Navigation.PushAsync(new AddClient(crudClients, selectedClient));
        }
    }
}
