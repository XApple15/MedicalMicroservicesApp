using DoctorService.Models.DTO.Doctor;
using MediatR;

namespace DoctorService.Application.Queries.Doctor
{
    public class SearchDoctorQueryDTO : IRequest<List<DoctorDTO>>
    {
        public string? Name { get; set; }
        public string? Specialty { get; set; }
    }
}
