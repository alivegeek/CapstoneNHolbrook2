using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CapstoneNHolbrook.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public ICommand AuthenticateCommand { get; }

        public ICommand UpdateReportsCommand { get; }

        public ICommand BackupDatabaseCommand { get; }
        public ICommand RestoreDatabaseCommand { get; }

        private readonly ICloudBackupRestore _cloudBackupRestore;
        public AppointmentSummaryReport AppointmentSummary { get; set; }
        private string _clientName;
        private string _currentSortButtonText = "All";
        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;
        private bool _isRefreshing;
        private ObservableCollection<ClientMetrics> _originalClientMetricsCollection;
        private string _searchText;
        private readonly object _lockObj = new object();
        private string currentSortOption = "All";
        private ObservableCollection<AppointmentViewModel> _allAppointments = new ObservableCollection<AppointmentViewModel>();
        private ObservableCollection<ClientMetrics> _clientMetricsCollection = new ObservableCollection<ClientMetrics>();
        public ObservableCollection<ClientMetrics> ClientMetricsCollection { get => _clientMetricsCollection; set { _clientMetricsCollection = value; OnPropertyChanged(); } }
        public ObservableCollection<AppointmentViewModel> Appointments { get; } = new ObservableCollection<AppointmentViewModel>();
        public ObservableCollection<Client> Clients { get; } = new ObservableCollection<Client>();
        public ObservableCollection<ReportItem> ReportItems { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand RefreshClientsCommand { get; }
        public ICommand ToggleSortCommand { get; }

        public string ClientName { get => _clientName; set { if (_clientName != value) { _clientName = value; OnPropertyChanged(); } } }
        public string CurrentSortButtonText { get => _currentSortButtonText; set { if (_currentSortButtonText != value) { _currentSortButtonText = value; OnPropertyChanged(); } } }
        public bool IsRefreshing { get => _isRefreshing; set { if (_isRefreshing == value) return; _isRefreshing = value; OnPropertyChanged(); } }
        public bool HideCancelled { get; set; } = false;
        public string SearchText { get => _searchText; set { if (_searchText != value) { _searchText = value; FilterClients(); } } }
        public event PropertyChangedEventHandler PropertyChanged;

        public DashboardViewModel(CRUDAppointments crudAppointments, CRUDClients crudClients)
        {
            _crudAppointments = crudAppointments;
            _crudClients = crudClients;

            RefreshCommand = new Command(async () => await RefreshAppointments());
            RefreshClientsCommand = new Command(async () => await RefreshClients());
            ToggleSortCommand = new Command(ToggleSortAppointments);

            _cloudBackupRestore = DependencyService.Get<ICloudBackupRestore>();
            BackupDatabaseCommand = new Command(async () => await BackupDatabaseAsync());
            RestoreDatabaseCommand = new Command(async () => await RestoreDatabaseAsync());
            LoadAppointments();
            LoadClients();
            LoadClientMetrics();
            UpdateReportsCommand = new Command(async () => await RefreshDataAndReports());

            _originalClientMetricsCollection = new ObservableCollection<ClientMetrics>(ClientMetricsCollection);
        }


        private async Task RefreshDataAndReports()
        {
            await RefreshAppointments();
            await RefreshClients();
        }


       
    
    private async Task LoadAppointments()
        {
            try
            {
                IsRefreshing = true;
                Appointments.Clear();
                _allAppointments.Clear();
                var appointments = await _crudAppointments.GetAllAsync();
                foreach (var appointment in appointments)
                {
                    if (appointment.Client != null)
                    {
                        var appointmentViewModel = new AppointmentViewModel { Appointment = appointment, AppointmentTime = appointment.AppointmentStartTime.ToString("HH:mm"), ClientName = $"{appointment.Client.FirstName} {appointment.Client.LastName}", TimeUntilAppointment = (appointment.AppointmentStartTime - DateTime.Now).ToString(@"dd\:hh\:mm") };
                        Appointments.Add(appointmentViewModel);
                        _allAppointments.Add(appointmentViewModel);
                    }
                    else
                    {
                        Debug.WriteLine($"appointment.Client is null for appointment ID: {appointment.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading appointments: {ex.Message}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task RefreshAppointments()
        {
            await LoadAppointments();
            SortAppointments(currentSortOption);
            UpdateAppointments();
            UpdateReports();  // Update reports after refreshing appointments
        }



        private async Task BackupDatabaseAsync()
        {
            try
            {
                await _cloudBackupRestore.BackupDatabaseAsync();
                Debug.WriteLine("Database successfully backed up.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error backing up the database: {ex.Message}");
            }
        }

        private async Task RestoreDatabaseAsync()
        {
            try
            {
                await _cloudBackupRestore.RestoreDatabaseAsync();
                Debug.WriteLine("Database successfully restored.");
                // You might want to refresh the data after restoring
                await RefreshAppointments();
                await RefreshClients();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring the database: {ex.Message}");
            }
        }


        private async Task LoadClients()
        {
            IsRefreshing = true;
            Clients.Clear();

            // Await the result of GetAllAsync and GetClientAsync
            var clients = await _crudClients.GetAllAsync();
            foreach (var c in clients)
            {
                Clients.Add(c);
            }

            var client = await _crudClients.GetClientAsync(1);

            ClientName = client != null ? $"{client.FirstName} {client.LastName}" : "Client not found";
            _originalClientMetricsCollection = new ObservableCollection<ClientMetrics>(ClientMetricsCollection);

            IsRefreshing = false;
        }


        private async Task RefreshClients()
        {
            await LoadClients();
            LoadClientMetrics();
            UpdateReports();  // Update reports after refreshing clients
        }

        private void LoadClientMetrics()
        {
            lock (_lockObj)
            {
                var tempCollection = new ObservableCollection<ClientMetrics>(Clients.Select(client => new ClientMetrics { ClientData = client, }));
                ClientMetricsCollection = tempCollection;
                _originalClientMetricsCollection = new ObservableCollection<ClientMetrics>(ClientMetricsCollection);
            }
        }

        private void FilterClients()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                ClientMetricsCollection.Clear();
                foreach (var item in _originalClientMetricsCollection)
                {
                    ClientMetricsCollection.Add(item);
                }
            }
            else
            {
                var filteredList = _originalClientMetricsCollection.Where(c => c.ClientData.FirstName.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0 || c.ClientData.LastName.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                ClientMetricsCollection.Clear();
                foreach (var item in filteredList)
                {
                    ClientMetricsCollection.Add(item);
                }
            }
        }

        private void ToggleSortAppointments()
        {
            switch (currentSortOption)
            {
                case "All":
                    currentSortOption = "Today";
                    break;
                case "Today":
                    currentSortOption = "Week";
                    break;
                case "Week":
                    currentSortOption = "Month";
                    break;
                case "Month":
                    currentSortOption = "All";
                    break;
            }

            CurrentSortButtonText = currentSortOption;
            SortAppointments(currentSortOption);
        }

        private void SortAppointments(string sortOption)
        {
            ObservableCollection<AppointmentViewModel> sortedList = new ObservableCollection<AppointmentViewModel>();

            switch (sortOption)
            {
                case "All":
                    sortedList = new ObservableCollection<AppointmentViewModel>(_allAppointments.OrderBy(a => a.AppointmentTime));
                    break;
                case "Today":
                    sortedList = new ObservableCollection<AppointmentViewModel>(_allAppointments.Where(a => a.Appointment.AppointmentStartTime.Date == DateTime.Today).OrderBy(a => a.AppointmentTime));
                    break;
                case "Week":
                    sortedList = new ObservableCollection<AppointmentViewModel>(_allAppointments.Where(a => a.Appointment.AppointmentStartTime.Date >= DateTime.Today && a.Appointment.AppointmentStartTime.Date <= DateTime.Today.AddDays(7)).OrderBy(a => a.AppointmentTime));
                    break;
                case "Month":
                    sortedList = new ObservableCollection<AppointmentViewModel>(_allAppointments.Where(a => a.Appointment.AppointmentStartTime.Date >= DateTime.Today && a.Appointment.AppointmentStartTime.Date <= DateTime.Today.AddMonths(1)).OrderBy(a => a.AppointmentTime));
                    break;
            }

            Appointments.Clear();
            foreach (var appointment in sortedList)
            {
                Appointments.Add(appointment);
            }
        }

        public void ToggleHideCancelled()
        {
            HideCancelled = !HideCancelled;
            UpdateAppointments();
        }

        private void UpdateAppointments()
        {
            ObservableCollection<AppointmentViewModel> updatedList = HideCancelled ? new ObservableCollection<AppointmentViewModel>(_allAppointments.Where(a => !a.Appointment.IsCancelled)) : new ObservableCollection<AppointmentViewModel>(_allAppointments);
            Appointments.Clear();
            foreach (var appointment in updatedList)
            {
                Appointments.Add(appointment);
            }
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateReports()
        {
            // Completed Appointments
            int completedAppointments = Appointments.Count(a => a.Appointment.AppointmentEndTime < DateTime.Now && !a.Appointment.IsCancelled);

            // Cancelled Appointments
            int cancelledAppointments = Appointments.Count(a => a.Appointment.IsCancelled);

            // Upcoming Appointments
            int upcomingAppointments = Appointments.Count(a => a.Appointment.AppointmentStartTime > DateTime.Now);

            // Average Appointment Duration
            double averageAppointmentDuration = Appointments.Any(a => (a.Appointment.AppointmentEndTime - a.Appointment.AppointmentStartTime).TotalMinutes != 0)
                                                ? Appointments.Average(a => (a.Appointment.AppointmentEndTime - a.Appointment.AppointmentStartTime).TotalMinutes)
                                                : 0;

            // Most Booked Client
            var mostBookedClientQuery = Appointments.GroupBy(a => a.Appointment.Client.Id)
                                                    .OrderByDescending(g => g.Count())
                                                    .FirstOrDefault();
            string mostBookedClient = mostBookedClientQuery != null
                                      ? $"{mostBookedClientQuery.First().Appointment.Client.FirstName} {mostBookedClientQuery.First().Appointment.Client.LastName}"
                                      : "N/A";

            // Most Frequent Canceller
            var mostFrequentCancellerQuery = Appointments.Where(a => a.Appointment.IsCancelled)
                                                        .GroupBy(a => a.Appointment.Client.Id)
                                                        .OrderByDescending(g => g.Count())
                                                        .FirstOrDefault();
            string mostFrequentCanceller = mostFrequentCancellerQuery != null
                                          ? $"{mostFrequentCancellerQuery.First().Appointment.Client.FirstName} {mostFrequentCancellerQuery.First().Appointment.Client.LastName}"
                                          : "N/A";

            // Update ReportItems collection with new metric values
            ReportItems = new ObservableCollection<ReportItem>
    {
        new ReportItem { Title = "Total Clients", Value = Clients.Count.ToString() },
        new ReportItem { Title = "Total Appointments", Value = Appointments.Count.ToString() },
        new ReportItem { Title = "Completed Appointments", Value = completedAppointments.ToString() },
        new ReportItem { Title = "Cancelled Appointments", Value = cancelledAppointments.ToString() },
        new ReportItem { Title = "Upcoming Appointments", Value = upcomingAppointments.ToString() },
        new ReportItem { Title = "Average Appointment Duration (Minutes)", Value = averageAppointmentDuration.ToString("F2") },
        new ReportItem { Title = "Most Booked Client", Value = mostBookedClient },
        new ReportItem { Title = "Most Frequent Canceller", Value = mostFrequentCanceller }  // New Report Item
    };
            OnPropertyChanged(nameof(ReportItems));
        }

    }



    public class ReportItem
    {
        public string Title { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Title}: {Value}";
        }
    }

}
