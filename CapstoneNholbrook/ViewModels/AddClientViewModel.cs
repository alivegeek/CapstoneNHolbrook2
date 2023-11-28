using System.Windows.Input;
using Microsoft.Maui.Controls;
using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;
using Application = Microsoft.Maui.Controls.Application;

namespace CapstoneNHolbrook.ViewModels
{
    public class AddClientViewModel
    {
        private INavigation _navigation;
        private CRUDClients _crudClients;
        private Client _existingClient;

        public AddClientViewModel(INavigation navigation, CRUDClients crudClients, Client existingClient = null)
        {
            _navigation = navigation;
            _crudClients = crudClients;
            _existingClient = existingClient;

            if (existingClient != null)
            {
                FirstName = existingClient.FirstName;
                LastName = existingClient.LastName;
                PhoneNumber = existingClient.PhoneNumber;
                Email = existingClient.Email;
                CanText = existingClient.CanText;
                Notes = existingClient.Notes;
            }

            AddClientCommand = new Command(async () => await AddClientAsync());
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool CanText { get; set; }
        public string Notes { get; set; }

        public ICommand AddClientCommand { get; private set; }

        private async Task AddClientAsync()
        {
            // Validation for FirstName
            if (string.IsNullOrEmpty(FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "First name is required.", "OK");
                return;
            }

            // Validation for LastName
            if (string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Last name is required.", "OK");
                return;
            }

            // Validation for PhoneNumber
            Regex phoneNumpattern = new Regex(@"^\d{10}$");
            if (!phoneNumpattern.IsMatch(PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Phone number must be 10 digits.", "OK");
                return;
            }

            // Validation for Email
            Regex emailpattern = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!string.IsNullOrEmpty(Email) && !emailpattern.IsMatch(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Email is not valid.", "OK");
                return;
            }

            try
            {
                Debug.WriteLine($"Adding/Editing client: {FirstName} {LastName}");

                if (_existingClient == null)
                {
                    // Create a new client object
                    _existingClient = new Client();
                }

                // Update properties
                _existingClient.FirstName = FirstName;
                _existingClient.LastName = LastName;
                _existingClient.PhoneNumber = PhoneNumber;
                _existingClient.Email = Email;
                _existingClient.CanText = CanText;
                _existingClient.Notes = Notes;

                if (_existingClient.Id == 0)
                {
                    // Add the new client
                    await _crudClients.AddAsync(_existingClient);
                }
                else
                {
                    // Update the existing client
                    await _crudClients.UpdateAsync(_existingClient);
                }

                Debug.WriteLine($"Client added/updated successfully: {_existingClient.Id}");

                Debug.WriteLine("Popping the AddClient page...");
                await _navigation.PopAsync(); // Use await here for asynchronous navigation
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                Debug.WriteLine($"Error in AddClient: {ex.Message}");
            }
        }
    }
}
