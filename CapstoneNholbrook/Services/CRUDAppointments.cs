using CapstoneNHolbrook.Models;
using CapstoneNHolbrook.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CRUDAppointments
{
    private readonly DbOperations _context;

    public CRUDAppointments(DbOperations context)
    {
        _context = context;
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetCancelledAppointmentsCountAsync()
    {
        return await _context.Appointments.CountAsync(a => a.IsCancelled);
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _context.Appointments.Include(a => a.Client).ToListAsync();
    }

    public async Task<List<Appointment>> GetAllForClientAsync(int clientId)
    {
        return await _context.Appointments.Include(a => a.Client).Where(a => a.Client.Id == clientId).ToListAsync();
    }

    public async Task<bool> UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id)
    {
        return await _context.Appointments.Include(a => a.Client).FirstOrDefaultAsync(a => a.Id == id);
    }
}
