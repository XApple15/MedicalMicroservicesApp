namespace AppointmentService.Model.DTO
{
    public class AddAppointmentDTO
    {
        public Guid MedicUserId { get; set; }
        public Guid PacientUserId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Observations { get; set; }

        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public DateTime? ConsultationDate { get; set; }
    }
}
