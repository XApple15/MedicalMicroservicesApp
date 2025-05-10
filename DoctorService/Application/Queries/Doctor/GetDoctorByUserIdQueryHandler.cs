using DoctorService.Application.Commands.Schedule;
using DoctorService.Data;
using DoctorService.Models.DTO.Doctor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetDoctorByUserIdQueryHandler : IRequestHandler<GetDoctorByUserIdQuery, DoctorDTO>
    {
        private readonly DoctorDBContext _dbContext;

        public GetDoctorByUserIdQueryHandler(DoctorDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DoctorDTO> Handle(GetDoctorByUserIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _dbContext.Doctors
                .Where(d => d.UserId == request.UserId) // Searching by UserId
                .Include(d => d.Schedule)  // Include the schedule if needed
                .FirstOrDefaultAsync(cancellationToken);

            if (doctor == null)
            {
                return null; 
            }

            return new DoctorDTO
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                CvPath = doctor.CvPath,
                PhotoUrl = doctor.PhotoUrl,
                Specialties = doctor.Specialties,
                Schedule = doctor.Schedule.Select(s => new ScheduleEntryDTO
                {
                    Day = s.Day,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }).ToList()
            };
        }
    }
}
