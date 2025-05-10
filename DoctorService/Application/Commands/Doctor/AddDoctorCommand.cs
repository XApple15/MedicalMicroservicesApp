using DoctorService.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Application.Commands.Doctor
{
    public class AddDoctorCommand : IRequestHandler<AddDoctorDTO, Guid>
    {
        private readonly DoctorDBContext _context;

        public AddDoctorCommand(DoctorDBContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(AddDoctorDTO request, CancellationToken cancellationToken)
        {
            var doctor = new Models.Doctor
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FullName = request.FullName,
                Specialties = request.Specialties
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            return doctor.UserId;
        }
    }
}
