using MediatR;

namespace DoctorService.Application.Commands.Doctor
{
    public class DeleteDoctorDTO: IRequest<bool>
    {
        public Guid DoctorId { get; set; }
        public DeleteDoctorDTO(Guid doctorId)
        {
            DoctorId = doctorId;
        }
    }
    
}
