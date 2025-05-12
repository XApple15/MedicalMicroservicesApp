using PacientService.Model;
using PacientService.Model.DTO;
using PacientService.Repository;

namespace PacientService.Facade
{
    public class PacientFacadeService
    {
        private readonly IPacientRepository _repo;

        public PacientFacadeService(IPacientRepository repo)
        {
            _repo = repo;
        }

        public async Task<Pacient?> GetPatientByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Pacient>> GetAllPatientsAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Guid> CreatePatientAsync(CreatePacientDTO dto)
        {
            var patient = new Pacient
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                DoctorUserId = dto.DoctorUserId,
                FullName = dto.FullName
            };

            await _repo.AddAsync(patient);
            return patient.Id;
        }

        public async Task<bool> UpdatePatientAsync(Guid id, UpdatePacientDTO dto)
        {
            var patient = await _repo.GetByIdAsync(id);
            if (patient is null) return false;

            patient.FullName = dto.FullName;

            await _repo.UpdateAsync(patient);
            return true;
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            var patient = await _repo.GetByIdAsync(id);
            if (patient is null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }

}
