using MediatR;

namespace DoctorService.Application.Commands.Doctor
{
    public class DeleteDoctorDTO: IRequest<bool>
    {
        public Guid UserId { get; set; }
        public DeleteDoctorDTO(Guid userId)
        {
            UserId = userId;
        }
    }
    
}
