using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class CRUDClients
{
    private readonly DbOperations _context;

    public CRUDClients(DbOperations context)
    {
        _context = context;
    }

    public async Task AddAsync(Client client)
    {
        Debug.WriteLine("Add method in CRUDClients called");
        Debug.WriteLine($"Inserting client into database: {client.FirstName} {client.LastName}");
        await _context.Clients.AddAsync(client);
        int entitiesWritten = await _context.SaveChangesAsync();
        Debug.WriteLine($"{entitiesWritten} entities written to the database");
    }

    public async Task UpdateAsync(Client updatedClient)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Id == updatedClient.Id);
        if (existingClient != null)
        {
            existingClient.FirstName = updatedClient.FirstName;
            existingClient.LastName = updatedClient.LastName;
            existingClient.PhoneNumber = updatedClient.PhoneNumber;
            existingClient.Email = updatedClient.Email;
            existingClient.CanText = updatedClient.CanText;
            existingClient.Notes = updatedClient.Notes;
            await _context.SaveChangesAsync();
        }
        else
        {
            Debug.WriteLine("Client not found for update.");
        }
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client> GetClientAsync(int id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    }
}
