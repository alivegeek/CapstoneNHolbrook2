using System;

namespace CapstoneNHolbrook.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public DateTime AppointmentStartTime { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public string TypeOfService { get; set; }
        public string Notes { get; set; }
        public bool IsCancelled { get; set; }  // New property
    }
}
