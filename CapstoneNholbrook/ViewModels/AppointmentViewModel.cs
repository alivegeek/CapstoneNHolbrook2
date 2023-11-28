using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CapstoneNHolbrook.Models;

namespace CapstoneNHolbrook.ViewModels
{
    public class AppointmentViewModel : INotifyPropertyChanged
    {
        private Appointment _appointment;

        public Appointment Appointment
        {
            get => _appointment;
            set
            {
                if (_appointment != value)
                {
                    _appointment = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsCancelled));
                }
            }
        }

        public string AppointmentTime { get; set; }
        public string ClientName { get; set; }
        public string TimeUntilAppointment { get; set; }
        public string Date { get; set; }
        public string Notes { get; set; }

        public bool IsCancelled => Appointment.IsCancelled;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateFromAppointment(Appointment updatedAppointment)
        {
            // Update properties based on the updated Appointment object
            this.Appointment = updatedAppointment;
            this.AppointmentTime = updatedAppointment.AppointmentStartTime.ToString();  // Convert DateTime to string if needed
            this.ClientName = updatedAppointment.Client.FullName;  // Assuming Client is not null
            // Add more properties to update if needed
        }

        // ... other existing properties and methods
    }
}
