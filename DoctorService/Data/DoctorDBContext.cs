using DoctorService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data
{
    public class DoctorDBContext:DbContext
    {
       public DoctorDBContext(DbContextOptions<DoctorDBContext> options) : base(options)
        { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<ScheduleEntry> ScheduleEntries { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Schedule)
                .WithOne(s => s.Doctor)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
