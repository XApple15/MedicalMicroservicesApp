using PacientService.Model;

namespace PacientService.Repository
{
    public interface IPacientRepository
    {
        Task<Pacient?> GetByIdAsync(Guid id);
        Task<IEnumerable<Pacient>> GetAllAsync();
        Task AddAsync(Pacient pacient);
        Task UpdateAsync(Pacient pacient);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsByUserIdAsync(Guid userId);
    }
}