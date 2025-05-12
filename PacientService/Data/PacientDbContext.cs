using Microsoft.EntityFrameworkCore;
using PacientService.Model;

namespace PacientService.Data
{
    public class PacientDbContext: DbContext
    {
        public PacientDbContext(DbContextOptions<PacientDbContext> options): base(options){}
        public DbSet<Pacient> Pacients { get; set; } 
    }
}
