using Microsoft.Maui.Controls;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.ViewModels;
using CapstoneNHolbrook.Models;

namespace CapstoneNHolbrook.Views
{
    public partial class AddClient : ContentPage
    {
        public AddClient(CRUDClients crudClients, Client client = null)
        {
            InitializeComponent();
            BindingContext = new AddClientViewModel(Navigation, crudClients, client);
        }
    }
}
