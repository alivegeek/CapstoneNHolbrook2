using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Linq;
using CapstoneNHolbrook.Models;
using System.Diagnostics;

namespace CapstoneNHolbrook.Services
{
    public class DbOperations : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ColorCard> ColorCards { get; set; }

        // Add this constructor
        public DbOperations(DbContextOptions<DbOperations> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string path = Path.Combine(FileSystem.Current.AppDataDirectory, "AppDatabase.db");
                optionsBuilder.UseSqlite($"Filename={path}");
            }
        }
        public void InitializeDatabase()
        {
            // Create the database if it doesn't exist
            Database.EnsureCreated();

            // Log the database file location
            string dbPath = Path.Combine(FileSystem.Current.AppDataDirectory, "AppDatabase.db");
            Debug.WriteLine($"Database file is located at: {dbPath}");
        }

        public void SeedTestData()
        {
            // The implementation of the seed data generation can be implemented here
        }

        public void GenerateSeedData()
        {
            // Empty the database
            EmptyDatabase();

            // Add test clients and other entities as needed
            // ...

            // Save the changes
            SaveChanges();
        }

        public void EmptyDatabase()
        {
            Appointments.RemoveRange(Appointments);
            Clients.RemoveRange(Clients);
            ColorCards.RemoveRange(ColorCards);
            SaveChanges();
        }
    }
}
