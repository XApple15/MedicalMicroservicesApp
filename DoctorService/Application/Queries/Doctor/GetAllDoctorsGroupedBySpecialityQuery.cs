using DoctorService.Models.DTO.Doctor;
using MediatR;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetAllDoctorsGroupedBySpecialityQuery: IRequest<Dictionary<string, List<DoctorDTO>>>
    {
    }
}
