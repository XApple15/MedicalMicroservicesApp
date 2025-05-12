using AppointmentService.Model;

namespace AppointmentService.Repository
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<List<Appointment>> GetAllAsync();
        Task AddAsync(Appointment appointment);
        Task<bool> DeleteAsync(Guid id);
        Task UpdateAsync(Appointment appointment);
    }

}
