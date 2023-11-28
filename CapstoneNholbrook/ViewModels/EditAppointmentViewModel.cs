using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls; // Updated namespace

namespace CapstoneNHolbrook.ViewModels
{
    public class EditAppointmentViewModel
    {
        public Client SelectedClient { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string TypeOfService { get; set; }
        public string Notes { get; set; }

        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

        private readonly CRUDAppointments _crudAppointments;
        private readonly Appointment _appointmentToEdit;

        public ICommand UpdateAppointmentCommand { get; set; }

        public EditAppointmentViewModel(CRUDAppointments crudAppointments, CRUDClients crudClients, Appointment appointmentToEdit)
        {
            _crudAppointments = crudAppointments ?? throw new ArgumentNullException(nameof(crudAppointments));
            _appointmentToEdit = appointmentToEdit ?? throw new ArgumentNullException(nameof(appointmentToEdit));

            SelectedClient = appointmentToEdit.Client;
            AppointmentDate = appointmentToEdit.AppointmentStartTime.Date;
            StartTime = appointmentToEdit.AppointmentStartTime.TimeOfDay;
            EndTime = appointmentToEdit.AppointmentEndTime.TimeOfDay;
            TypeOfService = appointmentToEdit.TypeOfService;
            Notes = appointmentToEdit.Notes;

            UpdateAppointmentCommand = new Command(async () => await UpdateAppointmentAsync());

            LoadClients(crudClients);
        }

        private async Task LoadClients(CRUDClients crudClients)
        {
            var clients = await crudClients.GetAllAsync();
            foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }

        private async Task UpdateAppointmentAsync()
        {
            try
            {
                Debug.WriteLine($"Updating appointment - Initial state: Client={_appointmentToEdit.Client.FullName}, Date={_appointmentToEdit.AppointmentStartTime}, Service={_appointmentToEdit.TypeOfService}");

                _appointmentToEdit.Client = SelectedClient;
                _appointmentToEdit.AppointmentStartTime = AppointmentDate.Add(StartTime);
                _appointmentToEdit.AppointmentEndTime = AppointmentDate.Add(EndTime);
                _appointmentToEdit.TypeOfService = TypeOfService;
                _appointmentToEdit.Notes = Notes;

                await _crudAppointments.UpdateAsync(_appointmentToEdit);

                Debug.WriteLine($"Updating appointment - Final state: Client={_appointmentToEdit.Client.FullName}, Date={_appointmentToEdit.AppointmentStartTime}, Service={_appointmentToEdit.TypeOfService}");

                // Update this line to use MAUI's navigation pattern if necessary
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while updating the appointment: {ex.Message}");
            }
        }
    }
}
