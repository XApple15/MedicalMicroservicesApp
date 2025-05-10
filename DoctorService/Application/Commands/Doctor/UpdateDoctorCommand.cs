using DoctorService.Data;
using DoctorService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Commands.Doctor
{
    public class UpdateDoctorCommand: IRequestHandler<UpdateDoctorDTO, bool>
    {
        private readonly DoctorDBContext _context;

        public UpdateDoctorCommand(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateDoctorDTO request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Schedule)
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor == null)
                return false;

            doctor.FullName = request.FullName;
            doctor.CvPath = request.CvPath;
            doctor.PhotoUrl = request.PhotoUrl;
            doctor.Specialties = request.Specialties;

            _context.ScheduleEntries.RemoveRange(doctor.Schedule);

            doctor.Schedule = request.Schedule.Select(entry => new ScheduleEntry
            {
                Id = Guid.NewGuid(),
                Day = (DayOfWeek)entry.Day,
                StartTime = entry.StartTime,
                EndTime = entry.EndTime,
                DoctorId = doctor.UserId
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
