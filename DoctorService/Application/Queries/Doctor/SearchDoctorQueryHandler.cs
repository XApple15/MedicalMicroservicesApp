using DoctorService.Application.Commands.Schedule;
using DoctorService.Data;
using DoctorService.Models.DTO.Doctor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Queries.Doctor
{
    public class SearchDoctorQueryHandler : IRequestHandler<SearchDoctorQueryDTO, List<DoctorDTO>>
    {
        private readonly DoctorDBContext _context;

        public SearchDoctorQueryHandler(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDTO>> Handle(SearchDoctorQueryDTO request, CancellationToken cancellationToken)
        {
            var query = _context.Doctors
                .Include(d => d.Schedule)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(d => d.FullName.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.Specialty))
            {
                query = query.Where(d => d.Specialties.Contains(request.Specialty));
            }

            return await query.Select(d => new DoctorDTO
            {
                Id = d.Id,
                UserId= d.UserId,
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
            }).ToListAsync(cancellationToken);
        }
    }
}
