using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls; // Updated namespace
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using System;
using System.Threading.Tasks;

namespace CapstoneNHolbrook.ViewModels
{
    public class ClientViewModel
    {
        public ObservableCollection<Client> Clients { get; set; }
        public Client SelectedClient { get; set; }
        public ICommand EditClientCommand { get; set; }
        public ICommand NewAppointmentCommand { get; set; }
        public ICommand ViewColorCardCommand { get; set; }

        private readonly CRUDClients _crudClients;

        public ClientViewModel(CRUDClients crudClients, Client selectedClient = null)
        {
            _crudClients = crudClients;

            // Async call to load the clients from the database
            LoadClientsAsync();

            // Initialize Commands
            EditClientCommand = new Command(EditClient);
            NewAppointmentCommand = new Command(ScheduleNewAppointment);
            ViewColorCardCommand = new Command(ViewColorCard);
        }

        private async void LoadClientsAsync()
        {
            var clientList = await _crudClients.GetAllAsync();
            Clients = new ObservableCollection<Client>(clientList);
        }


        void EditClient()
        {
            // Edit Client logic here
            // You might want to navigate to an EditClientPage
        }

        void ScheduleNewAppointment()
        {
            // New Appointment logic here
            // You might want to navigate to a NewAppointmentPage
        }

        void ViewColorCard()
        {
            // View Color Card logic here
            // You might want to navigate to a ColorCardPage
        }
    }
}
