using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls;
using CapstoneNHolbrook.Services;
using CapstoneNHolbrook.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using CapstoneNHolbrook; // Make sure this is the correct namespace where MauiProgram is located

namespace CapstoneNHolbrook
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register the database context
            builder.Services.AddDbContext<DbOperations>(options =>
                options.UseSqlite($"Filename={Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "AppDatabase.db")}"));

            // Register services
            builder.Services.AddSingleton<CRUDAppointments>();
            builder.Services.AddSingleton<CRUDClients>();
            builder.Services.AddSingleton<CRUDColorCards>();

            // Ensure the database is created and potentially seed data
            var dbOptions = new DbContextOptionsBuilder<DbOperations>()
                .UseSqlite($"Filename={Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "AppDatabase.db")}")
                .Options;
            using (var db = new DbOperations(dbOptions))
            {
                db.Database.EnsureCreated();
                // db.SeedTestData(); // Uncomment if seeding data on startup is desired
            }

            return builder.Build();
        }
    }
}
