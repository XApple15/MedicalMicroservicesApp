using PacientService.Model;
using System;
using PacientService.Data;
using Microsoft.EntityFrameworkCore;

namespace PacientService.Repository
{
    public class PacientRepository : IPacientRepository
    {
        private readonly PacientDbContext _context;

        public PacientRepository(PacientDbContext context)
        {
            _context = context;
        }

        public async Task<Pacient?> GetByIdAsync(Guid id)
        {
            return await _context.Pacients
                .FirstOrDefaultAsync(d => d.UserId == id);
        }

        public async Task<IEnumerable<Pacient>> GetAllAsync()
        {
            return await _context.Pacients.ToListAsync();
        }

        public async Task AddAsync(Pacient patient)
        {
            await _context.Pacients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pacient patient)
        {
            _context.Pacients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await _context.Pacients
                .FirstOrDefaultAsync(d => d.UserId == id); ;
            if (patient != null)    
            {
                _context.Pacients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByUserIdAsync(Guid userId)
        {
            return await _context.Pacients.AnyAsync(p => p.UserId == userId);
        }
    }
}
