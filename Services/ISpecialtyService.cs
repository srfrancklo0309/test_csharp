using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public interface ISpecialtyService
    {
        Task<List<Specialty>> GetAllAsync();
        Task<Specialty?> GetByIdAsync(int id);
        Task<Specialty> CreateAsync(Specialty specialty);
        Task<Specialty> UpdateAsync(Specialty specialty);
        Task<bool> DeleteAsync(int id);
    }
}
