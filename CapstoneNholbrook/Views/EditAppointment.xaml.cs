using System;
using System.Linq;
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.ViewModels;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.Views
{
    public partial class EditAppointment : ContentPage
    {
        public EditAppointment(CRUDAppointments crudAppointments, CRUDClients crudClients, Appointment appointmentToEdit)
        {
            InitializeComponent();

            // Set the MinimumDate property of the DatePicker
            this.FindByName<DatePicker>("AppointmentDatePicker").MinimumDate = DateTime.Now;

            // Set the BindingContext to a new instance of EditAppointmentViewModel
            BindingContext = new EditAppointmentViewModel(crudAppointments, crudClients, appointmentToEdit);
        }
    }
}
