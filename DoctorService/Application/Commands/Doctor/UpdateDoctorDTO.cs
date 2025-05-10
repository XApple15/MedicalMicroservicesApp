using DoctorService.Application.Commands.Schedule;
using MediatR;

namespace DoctorService.Application.Commands.Doctor
{
    public class UpdateDoctorDTO: IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? CvPath { get; set; }
        public string? PhotoUrl { get; set; }
        public List<string> Specialties { get; set; }
        public List<ScheduleEntryDto> Schedule { get; set; }
    }
}
