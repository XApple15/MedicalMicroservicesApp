using MediatR;

namespace DoctorService.Application.Commands.Doctor
{
    public class AddDoctorDTO : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public List<string> Specialties { get; set; }
        public string? CvPath { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
