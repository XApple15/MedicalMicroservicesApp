using AppointmentService.Model;
using AppointmentService.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Service
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAsync(AddAppointmentDTO dto);
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, UpdateAppointmentDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<Appointment>?> SearchAppointmentsAsync(Guid? pacientUserId,Guid? doctorUserId,string? diagnosis);
    }

}
