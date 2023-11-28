using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.ViewModels;
using CapstoneNHolbrook.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using System;

namespace CapstoneNHolbrook
{
    public partial class App : Application
    {
        // Expose the service provider to the rest of the application
        public IServiceProvider Services { get; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Store the service provider
            Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Resolve services using the service provider
            var crudAppointments = Services.GetRequiredService<CRUDAppointments>();
            var crudClients = Services.GetRequiredService<CRUDClients>();
            var dashboardViewModel = Services.GetRequiredService<DashboardViewModel>();

            // Set the main page of the app to be the MainPage from the Views namespace
            MainPage = new NavigationPage(new Views.MainPage(crudAppointments, crudClients, dashboardViewModel));
        }

        // Add override methods if necessary
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
