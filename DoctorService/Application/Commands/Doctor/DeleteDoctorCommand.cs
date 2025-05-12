using DoctorService.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Commands.Doctor
{
    public class DeleteDoctorCommand : IRequestHandler<DeleteDoctorDTO,bool>
    {
        private readonly DoctorDBContext _context;

        public DeleteDoctorCommand(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteDoctorDTO request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Schedule) 
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor == null)
                return false;

            _context.Doctors.Remove(doctor);

            _context.ScheduleEntries.RemoveRange(doctor.Schedule);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
