using CapstoneNHolbrook.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneNHolbrook.Services
{
    public class ClientMetrics
    {
        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;

        public Client ClientData { get; set; }

        public ClientMetrics() { }

        public ClientMetrics(CRUDAppointments crudAppointments, CRUDClients crudClients)
        {
            _crudAppointments = crudAppointments;
            _crudClients = crudClients;
        }

        public async Task<AppointmentSummaryReport> GenerateAppointmentSummaryReportAsync()
        {
            var appointments = await _crudAppointments.GetAllAsync();
            var clients = await _crudClients.GetAllAsync();
            var totalAppointments = appointments.Count;
            var completedAppointments = appointments.Count(a => a.AppointmentEndTime < DateTime.Now);
            var cancelledAppointments = appointments.Count(a => a.IsCancelled);
            var futureAppointments = appointments.Count(a => a.AppointmentStartTime > DateTime.Now);
            var avgAppointmentLength = totalAppointments > 0 ? appointments.Average(a => (a.AppointmentEndTime - a.AppointmentStartTime).TotalMinutes) : 0;
            var percentageCancelled = totalAppointments > 0 ? (double)cancelledAppointments / totalAppointments * 100 : 0;
            var mostFrequentClient = appointments
                                        .GroupBy(a => a.Client.Id)
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => g.Key)
                                        .FirstOrDefault();

            return new AppointmentSummaryReport
            {
                TotalAppointments = totalAppointments,
                CompletedAppointments = completedAppointments,
                CancelledAppointments = cancelledAppointments,
                WeeklyAppointments = appointments.Count(a => a.AppointmentStartTime >= DateTime.Now.AddDays(-7)),
                MonthlyAppointments = appointments.Count(a => a.AppointmentStartTime >= DateTime.Now.AddMonths(-1)),
                YearlyAppointments = appointments.Count(a => a.AppointmentStartTime >= DateTime.Now.AddYears(-1)),
                FutureAppointments = futureAppointments,
                AverageAppointmentLength = avgAppointmentLength,
                PercentageAppointmentsCancelled = percentageCancelled,
                TotalClients = clients.Count,
                MostFrequentClient = mostFrequentClient
            };
        }
    }

    public class AppointmentSummaryReport
    {
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int WeeklyAppointments { get; set; }
        public int MonthlyAppointments { get; set; }
        public int YearlyAppointments { get; set; }
        public int FutureAppointments { get; set; }
        public double AverageAppointmentLength { get; set; }
        public double PercentageAppointmentsCancelled { get; set; }
        public int TotalClients { get; set; }
        public int MostFrequentClient { get; set; }
    }
}
