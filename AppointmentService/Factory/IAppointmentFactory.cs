using AppointmentService.Model;

namespace AppointmentService.Factory
{
    public interface IAppointmentFactory
    {
        Appointment CreateAppointment(Guid medicUserId, Guid pacientUserId, DateTime date, string? notes = null);
    }
}
