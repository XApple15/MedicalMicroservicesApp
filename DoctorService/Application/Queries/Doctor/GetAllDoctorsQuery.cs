using DoctorService.Models.DTO.Doctor;
using MediatR;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetAllDoctorsQuery:IRequest<List<DoctorDTO>>
    {
    }
}
