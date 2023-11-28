using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Views;
using CapstoneNHolbrook.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace CapstoneNHolbrook.ViewModels
{
    public class ClientPageViewModel : INotifyPropertyChanged
    {
        private readonly Client _client;
        private readonly INavigation _navigation;
        private readonly CRUDAppointments _crudAppointments;
        private readonly CRUDClients _crudClients;
        private readonly CRUDColorCards _crudColorCards;

        public string Name => _client?.FullName ?? "N/A";
        public string Email => _client?.Email ?? "N/A";
        public string PhoneNumber => _client?.PhoneNumber ?? "N/A";
        public string CanText => _client?.CanText.ToString() ?? "N/A";
        public string IsActive => _client?.IsActive.ToString() ?? "N/A";
        public string Notes => _client?.Notes ?? "N/A";

        public ClientPageViewModel(
            Client client,
            INavigation navigation,
            CRUDAppointments crudAppointments,
            CRUDClients crudClients,
            CRUDColorCards crudColorCards)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _crudAppointments = crudAppointments ?? throw new ArgumentNullException(nameof(crudAppointments));
            _crudClients = crudClients ?? throw new ArgumentNullException(nameof(crudClients));
            _crudColorCards = crudColorCards ?? throw new ArgumentNullException(nameof(crudColorCards));
        }

        public Command ViewColorCardCommand => new Command(async () => await ViewColorCard());
        public Command NewAppointmentCommand => new Command(async () => await NewAppointment());
        public Command EditClientCommand => new Command(async () => await EditClient());

        private async Task ViewColorCard()
        {
            await _navigation.PushAsync(new ColorCardsPage(_client, _crudColorCards));
        }

        private async Task NewAppointment()
        {
            await _navigation.PushAsync(new AddAppointment(_crudAppointments, _crudClients, _client)); // Use the correct page name
        }

        private async Task EditClient()
        {
            await _navigation.PushAsync(new AddClient(_crudClients, _client)); // Use the correct page name
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
