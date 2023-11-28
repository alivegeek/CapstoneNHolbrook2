using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.Views;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.ViewModels
{
    public class AppointmentDetailViewModel
    {
        private readonly INavigation _navigation;
        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;
        private readonly Appointment _appointment;

        public string AppointmentTime { get; set; }
        public string ClientName { get; set; }
        public string TimeUntilAppointment { get; set; }
        public string Date { get; set; }
        public string Notes { get; set; }
        public string ServiceType { get; set; }
        public Appointment Appointment { get; set; }

        public ICommand EditAppointmentCommand { get; set; }
        public ICommand CancelAppointmentCommand { get; set; }

        public AppointmentDetailViewModel(AppointmentViewModel selectedAppointment, CRUDAppointments crudAppointments, CRUDClients crudClients, INavigation navigation)
        {
            _crudAppointments = crudAppointments ?? throw new ArgumentNullException(nameof(crudAppointments));
            _crudClients = crudClients ?? throw new ArgumentNullException(nameof(crudClients));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            if (selectedAppointment == null)
            {
                throw new ArgumentNullException(nameof(selectedAppointment));
            }

            Appointment = selectedAppointment.Appointment ?? throw new ArgumentNullException(nameof(selectedAppointment.Appointment));
            AppointmentTime = selectedAppointment.AppointmentTime ?? throw new ArgumentNullException(nameof(selectedAppointment.AppointmentTime));
            ClientName = selectedAppointment.ClientName ?? throw new ArgumentNullException(nameof(selectedAppointment.ClientName));
            TimeUntilAppointment = selectedAppointment.TimeUntilAppointment ?? throw new ArgumentNullException(nameof(selectedAppointment.TimeUntilAppointment));
            Date = selectedAppointment.Date;

            Notes = Appointment.Notes;
            ServiceType = Appointment.TypeOfService;

            EditAppointmentCommand = new Command(async () => await OnEditAppointment());
            CancelAppointmentCommand = new Command(async () => await OnCancelAppointment());
        }

        private async Task OnEditAppointment()
        {
            var appointmentToEdit = await _crudAppointments.GetAppointmentByIdAsync(Appointment.Id);
            if (appointmentToEdit != null)
            {
                await _navigation.PushAsync(new EditAppointment(_crudAppointments, _crudClients, appointmentToEdit));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not find the appointment to edit.", "OK");
            }
        }

        private async Task OnCancelAppointment()
        {
            // Toggle the IsCancelled property
            Appointment.IsCancelled = !Appointment.IsCancelled;

            // Update the database
            await _crudAppointments.UpdateAsync(Appointment);  // Notice the method name change here to UpdateAsync

            // Refresh the UI
            MessagingCenter.Send(this, "RefreshView");
        }
    }
}
