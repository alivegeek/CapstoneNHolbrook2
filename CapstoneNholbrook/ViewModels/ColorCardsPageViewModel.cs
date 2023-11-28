using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls; // Updated namespace

namespace CapstoneNHolbrook.ViewModels
{
    public class ColorCardsPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ColorCard> _colorCards;
        private ColorCard _selectedColorCard;
        private readonly CRUDColorCards _crudColorCards;
        private readonly INavigation _navigation;
        private readonly Client _client;
        private bool _isBusy;

        public ObservableCollection<ColorCard> ColorCards
        {
            get => _colorCards;
            set
            {
                _colorCards = value;
                OnPropertyChanged();
            }
        }

        public ColorCard SelectedColorCard
        {
            get => _selectedColorCard;
            set
            {
                if (_selectedColorCard != value)
                {
                    _selectedColorCard = value;
                    OnPropertyChanged();
                    NavigateToColorCardDetailPage(value);
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddColorCardCommand => new Command(NavigateToAddColorCard);
        public ICommand RefreshCommand => new Command(RefreshData);

        public ColorCardsPageViewModel(Client client, INavigation navigation, CRUDColorCards crudColorCards)
        {
            _client = client;
            _navigation = navigation;
            _crudColorCards = crudColorCards;
            LoadColorCards();
        }

        private async void LoadColorCards()
        {
            var colorCards = await _crudColorCards.GetColorCardsByClientAsync(_client.Id);
            ColorCards = new ObservableCollection<ColorCard>(colorCards);
        }


        private async void NavigateToAddColorCard()
        {
            // Update the navigation method if required for MAUI
            await _navigation.PushAsync(new AddColorCardPage(_client, _crudColorCards));
        }

        private async void NavigateToColorCardDetailPage(ColorCard selectedColorCard)
        {
            // Update the navigation method if required for MAUI
            await _navigation.PushAsync(new ColorCardDetailPage(selectedColorCard, _crudColorCards));
        }

        private void RefreshData()
        {
            IsBusy = true;
            LoadColorCards();
            IsBusy = false;
        }

        public void ReloadData()
        {
            LoadColorCards();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
