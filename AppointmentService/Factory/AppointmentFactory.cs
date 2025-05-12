using AppointmentService.Model;

namespace AppointmentService.Factory
{
    public class AppointmentFactory : IAppointmentFactory
    {
        public Appointment CreateAppointment(Guid medicUserId, Guid pacientUserId, DateTime date, string? notes = null)
        {
            if (date <= DateTime.Now)
                throw new ArgumentException("Appointment date must be in the future.");

            return new Appointment
            {
                Id = Guid.NewGuid(),
                MedicUserId = medicUserId,
                PacientUserId = pacientUserId,
                AppointmentDateTime = date,
                Status = "Scheduled",
                Observations = notes
            };
        }
    }

}
