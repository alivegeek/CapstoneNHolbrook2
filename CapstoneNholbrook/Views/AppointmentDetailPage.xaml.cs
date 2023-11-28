using System;
using CapstoneNHolbrook.ViewModels;
using CapstoneNHolbrook.Services;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.Views
{
    public partial class AppointmentDetailPage : ContentPage
    {
        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;
        private AppointmentViewModel _selectedAppointment;

        public AppointmentDetailPage(AppointmentViewModel selectedAppointment, CRUDAppointments crudAppointments, CRUDClients crudClients)
        {
            InitializeComponent();

            _crudAppointments = crudAppointments;
            _crudClients = crudClients;
            _selectedAppointment = selectedAppointment;

            BindingContext = new AppointmentDetailViewModel(_selectedAppointment, _crudAppointments, _crudClients, Navigation);

            // Subscribe to the MessagingCenter to refresh the view
            MessagingCenter.Subscribe<AppointmentDetailViewModel>(this, "RefreshView", (sender) =>
            {
                OnAppearing();
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var updatedAppointment = await _crudAppointments.GetAppointmentByIdAsync(_selectedAppointment.Appointment.Id);
            _selectedAppointment.UpdateFromAppointment(updatedAppointment);

            // Update the view model if necessary
            var viewModel = (AppointmentDetailViewModel)BindingContext;
            if (viewModel != null)
            {
                viewModel.Appointment = updatedAppointment;
                CancelledLabel.IsVisible = updatedAppointment.IsCancelled;
            }
        }

        // Unsubscribe from the MessagingCenter when the page disappears
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AppointmentDetailViewModel>(this, "RefreshView");
        }
    }
}
