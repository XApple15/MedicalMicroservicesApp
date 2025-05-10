using DoctorService.Models.DTO.Doctor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Application.Queries.Doctor
{
    public class GetDoctorByUserIdQuery : IRequest<DoctorDTO>
    {
        public Guid UserId { get; set; }

        public GetDoctorByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
