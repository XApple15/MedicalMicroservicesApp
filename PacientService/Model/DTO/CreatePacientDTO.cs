namespace PacientService.Model.DTO
{
    public class CreatePacientDTO
    {
        public Guid UserId { get; set; }
        public Guid DoctorUserId { get; set; }
        public string FullName { get; set; } = string.Empty;  
    }
}
