using MediatR;

namespace DoctorService.Application.Commands.Schedule
{
    public class AddScheduleDTO : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
