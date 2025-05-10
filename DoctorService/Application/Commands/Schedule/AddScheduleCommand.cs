using DoctorService.Data;
using DoctorService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Application.Commands.Schedule
{
    public class AddScheduleCommand : IRequestHandler<AddScheduleDTO, bool>
    {
        private readonly DoctorDBContext _dbContext;

        public AddScheduleCommand(DoctorDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(AddScheduleDTO request, CancellationToken cancellationToken)
        {
            var doctor = await _dbContext.Doctors
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

            if (doctor == null)
            {
                return false; 
            }

            var scheduleEntry = new ScheduleEntry
            {
                DoctorId = doctor.UserId,
                Day = request.Day,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            await _dbContext.ScheduleEntries.AddAsync(scheduleEntry, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
