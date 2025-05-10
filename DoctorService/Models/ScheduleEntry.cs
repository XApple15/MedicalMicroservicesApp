using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Models
{
    public class ScheduleEntry
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; } 
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Doctor Doctor { get; set; } 
    }
}
