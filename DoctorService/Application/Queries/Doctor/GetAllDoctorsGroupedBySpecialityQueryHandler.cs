using DoctorService.Application.Commands.Schedule;
using DoctorService.Data;
using DoctorService.Models.DTO.Doctor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetAllDoctorsGroupedBySpecialityQueryHandler : IRequestHandler<GetAllDoctorsGroupedBySpecialityQuery, Dictionary<string, List<DoctorDTO>>>
    {
        private readonly DoctorDBContext _context;

        public GetAllDoctorsGroupedBySpecialityQueryHandler(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, List<DoctorDTO>>> Handle(GetAllDoctorsGroupedBySpecialityQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _context.Doctors
                .Include(d => d.Schedule)
                .ToListAsync(cancellationToken);

            var doctorDtos = doctors.Select(d => new DoctorDTO
            {
                Id = d.Id,
                UserId = d.UserId,
                FullName = d.FullName,
                Specialties = d.Specialties,
                CvPath = d.CvPath,
                PhotoUrl = d.PhotoUrl,
                Schedule = d.Schedule.Select(s => new ScheduleEntryDTO
                {
                    Day = s.Day,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).ToList()
            }).ToList();
            var doctorsBySpecialty = doctorDtos
                .SelectMany(d => d.Specialties.Select(s => new { Specialty = s, Doctor = d }))
                .GroupBy(x => x.Specialty)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(x => x.Doctor).ToList() 
                );

           
            return doctorsBySpecialty;
        }
    }
}


