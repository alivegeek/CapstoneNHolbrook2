using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.ViewModels
{
    public class AddAppointmentViewModel : BindableObject
    {
        public ObservableCollection<Client> Clients { get; }
        private  List<Client> _allClients;
        private Client _selectedClient;
        private DateTime _appointmentDate;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private string _typeOfService;

        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;

        public ICommand AddAppointmentCommand { get; }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public DateTime AppointmentDate
        {
            get => _appointmentDate;
            set
            {
                _appointmentDate = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public string TypeOfService
        {
            get => _typeOfService;
            set
            {
                _typeOfService = value;
                OnPropertyChanged();
            }
        }

        public AddAppointmentViewModel(CRUDAppointments crudAppointments, CRUDClients crudClients, Client selectedClient = null)
        {
            _crudAppointments = crudAppointments;
            _crudClients = crudClients;
            _allClients = new List<Client>();
            Clients = new ObservableCollection<Client>();
            LoadClients();

            AddAppointmentCommand = new Command(async () => await AddAppointmentAsync());

            SelectedClient = selectedClient ?? new Client();
            AppointmentDate = DateTime.Today;
            StartTime = new TimeSpan(12, 0, 0);
            EndTime = new TimeSpan(13, 0, 0);
            TypeOfService = string.Empty;
        }

        private async Task LoadClients()
        {
            var clients = await _crudClients.GetAllAsync();
            _allClients = new List<Client>(clients);
            Clients.Clear();
            foreach (var client in _allClients)
            {
                Clients.Add(client);
            }
        }

        public async Task AddAppointmentAsync()
        {
            if (EndTime <= StartTime)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Time", "The end time must be after the start time.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(TypeOfService))
            {
                await Application.Current.MainPage.DisplayAlert("Missing Information", "Type of Service cannot be empty.", "OK");
                return;
            }

            var appointment = new Appointment
            {
                Client = SelectedClient,
                AppointmentStartTime = AppointmentDate.Add(StartTime),
                AppointmentEndTime = AppointmentDate.Add(EndTime),
                TypeOfService = TypeOfService
            };

            await _crudAppointments.AddAsync(appointment);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public void FilterClients(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Clients.Clear();
                foreach (var client in _allClients)
                {
                    Clients.Add(client);
                }
            }
            else
            {
                var filteredClients = _allClients
                                      .Where(c => c.FirstName.ToLower().Contains(searchText.ToLower()) || c.LastName.ToLower().Contains(searchText.ToLower()))
                                      .ToList();
                Clients.Clear();
                foreach (var client in filteredClients)
                {
                    Clients.Add(client);
                }
            }
        }
    }
}
