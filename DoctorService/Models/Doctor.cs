namespace DoctorService.Models
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 

        public string FullName { get; set; }
       
        public List<string> Specialties { get; set; } 
        public string? CvPath { get; set; }
        public string? PhotoUrl { get; set; }

        public ICollection<ScheduleEntry> Schedule { get; set; } 
    }
}
