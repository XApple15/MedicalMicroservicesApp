namespace AppointmentService.Model.DTO
{
    public class UpdateAppointmentDTO
    {
        public string Status { get; set; }
        public string? Observations { get; set; }

        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public DateTime? ConsultationDate { get; set; }
    }
}
