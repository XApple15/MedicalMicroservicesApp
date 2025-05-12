using AppointmentService.Factory;
using AppointmentService.Model;
using AppointmentService.Model.DTO;
using AppointmentService.Repository;

namespace AppointmentService.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IAppointmentFactory _factory;

        public AppointmentService(IAppointmentRepository repo, IAppointmentFactory factory)
        {
            _repo = repo;
            _factory = factory;
        }

        public async Task<Appointment> CreateAsync(AddAppointmentDTO dto)
        {
            var appointment = _factory.CreateAppointment(dto.MedicUserId, dto.PacientUserId, dto.AppointmentDateTime, dto.Observations);
            await _repo.AddAsync(appointment);
            return appointment;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateAppointmentDTO dto)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Status)) appointment.Status = dto.Status;
            if (!string.IsNullOrWhiteSpace(dto.Observations)) appointment.Observations = dto.Observations;
            if (!string.IsNullOrWhiteSpace(dto.Symptoms)) appointment.Symptoms = dto.Symptoms;
            if (!string.IsNullOrWhiteSpace(dto.Diagnosis)) appointment.Diagnosis = dto.Diagnosis;
            if (!string.IsNullOrWhiteSpace(dto.Treatment)) appointment.Treatment = dto.Treatment;
            if (dto.ConsultationDate != null) appointment.ConsultationDate = dto.ConsultationDate;

            await _repo.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

       public async Task<List<Appointment>?> SearchAppointmentsAsync(Guid? pacientUserId, Guid? doctorUserId, string? diagnosis)
        {
            var appointments = await _repo.GetAllAsync();
            if (appointments == null) return null;

            if (pacientUserId != null)
                appointments = appointments.Where(a => a.PacientUserId == pacientUserId).ToList();

            if (doctorUserId != null)
                appointments = appointments.Where(a => a.MedicUserId == doctorUserId).ToList();

            if (!string.IsNullOrWhiteSpace(diagnosis))
                appointments = appointments.Where(a => a.Diagnosis == diagnosis).ToList();

            return appointments;
        }
    }

}
