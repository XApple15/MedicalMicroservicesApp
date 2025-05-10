namespace DoctorService.Application.Commands.Schedule
{
    public class ScheduleEntryDto
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
