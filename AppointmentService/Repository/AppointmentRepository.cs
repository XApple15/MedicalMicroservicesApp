using AppointmentService.Data;
using AppointmentService.Model;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt == null)
                return false;

            _context.Appointments.Remove(appt);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }
    }

}
