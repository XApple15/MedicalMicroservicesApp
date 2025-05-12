namespace PacientService.Model
{
    public class Pacient
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DoctorUserId { get; set; }
        public String? FullName { get; set; }
    }
}
