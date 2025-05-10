namespace DoctorService.Application.Commands.Schedule
{
    public class ScheduleEntryDTO
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
