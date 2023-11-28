using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.ViewModels;
using System.Diagnostics;
using CapstoneNHolbrook.Models;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace CapstoneNHolbrook.Views
{
    public partial class MainPage : TabbedPage
    {
        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;
        private readonly CRUDColorCards _crudColorCards;
        private readonly DashboardViewModel _dashboardViewModel;

        public MainPage(CRUDAppointments crudAppointments, CRUDClients crudClients, CRUDColorCards crudColorCards, DashboardViewModel dashboardViewModel)
        {
            InitializeComponent();

            _crudAppointments = crudAppointments ?? throw new ArgumentNullException(nameof(crudAppointments));
            _crudClients = crudClients ?? throw new ArgumentNullException(nameof(crudClients));
            _crudColorCards = crudColorCards ?? throw new ArgumentNullException(nameof(crudColorCards));
            _dashboardViewModel = dashboardViewModel ?? throw new ArgumentNullException(nameof(dashboardViewModel));

            BindingContext = _dashboardViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                _dashboardViewModel?.RefreshClientsCommand.Execute(null);
                _dashboardViewModel?.RefreshCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during Refresh operations: {ex}");
                this.DisplayAlert("Error", "An error occurred during Refresh operations.", "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _dashboardViewModel.SearchText = e.NewTextValue;
        }

        public async void OnAddAppointmentClicked(object sender, EventArgs e)
        {
            try
            {
                var allClients = await _crudClients.GetAllAsync();
                if (allClients.Count > 0)
                {
                    Client selectedClient = allClients[0];
                    await Navigation.PushAsync(new AddAppointment(_crudAppointments, _crudClients, selectedClient));
                }
                else
                {
                    await this.DisplayAlert("Alert", "No clients available. Please add a client first.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while adding an appointment: {ex}");
                await this.DisplayAlert("Error", "An error occurred while adding an appointment.", "OK");
            }
        }

        private void OnAddClientClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddClient(_crudClients));
        }

        public async void OnClientItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem is ClientMetrics selectedClientMetrics)
                {
                    await Navigation.PushAsync(new ClientPage(selectedClientMetrics.ClientData, _crudAppointments, _crudClients, _crudColorCards));
                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while selecting a client: {ex}");
                await this.DisplayAlert("Error", "An error occurred while selecting a client.", "OK");
            }
        }

        public async void OnAppointmentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem is AppointmentViewModel selectedAppointment)
                {
                    if (selectedAppointment.Appointment == null)
                    {
                        Console.WriteLine("Debug: selectedAppointment.Appointment is null");
                        return;
                    }

                    await Navigation.PushAsync(new AppointmentDetailPage(selectedAppointment, _crudAppointments, _crudClients));
                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to appointment details: {ex.Message}");
                await this.DisplayAlert("Error", "An error occurred while navigating to appointment details.", "OK");
            }
        }

        public void OnHideCancelledClicked(object sender, EventArgs e)
        {
            _dashboardViewModel.ToggleHideCancelled();
        }

        public async Task LoadAllClientsAsync()
        {
            try
            {
                var clients = await _crudClients.GetAllAsync();
                // Process the clients as needed here
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while loading clients: {ex}");
                // Handle the error appropriately
            }
        }

        private async Task ExecuteAndHandleExceptionsAsync(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during execution: {ex}");
                await this.DisplayAlert("Error", "An error occurred during an operation.", "OK");
            }
        }
    }
}
