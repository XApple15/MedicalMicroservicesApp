using DoctorService.Application.Commands.Schedule;
using DoctorService.Data;
using DoctorService.Models.DTO.Doctor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, List<DoctorDTO>>
    {
        private readonly DoctorDBContext _context;

        public GetAllDoctorsQueryHandler(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDTO>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _context.Doctors
                .Include(d => d.Schedule) 
                .ToListAsync(cancellationToken);

            var doctorDtos = doctors.Select(d => new DoctorDTO
            {
                Id = d.Id,
                UserId = d.UserId,
                FullName = d.FullName,
                CvPath = d.CvPath,
                PhotoUrl = d.PhotoUrl,
                Specialties = d.Specialties,
                Schedule = d.Schedule.Select(s => new ScheduleEntryDTO
                {
                    Day = s.Day,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).ToList()
            }).ToList();

            return doctorDtos;
        }
    }
}