using DoctorService.Application.Commands.Schedule;

namespace DoctorService.Models.DTO.Doctor
{
    public class DoctorDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public List<string> Specialties { get; set; }
        public string? CvPath { get; set; }
        public string? PhotoUrl { get; set; }

        public List<ScheduleEntryDTO> Schedule { get; set; }
    }
}
