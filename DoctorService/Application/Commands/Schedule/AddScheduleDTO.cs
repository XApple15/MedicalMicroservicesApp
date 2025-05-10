using MediatR;

namespace DoctorService.Application.Commands.Schedule
{
    public class AddScheduleDTO : IRequest<bool>
    {
        public Guid DoctorId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
