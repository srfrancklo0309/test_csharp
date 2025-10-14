using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public interface IDoctorService
    {
        Task<Doctor?> GetByIdAsync(long id);
        Task<Doctor?> GetByDocumentAsync(string document);
        Task<List<Doctor>> GetAllAsync();
        Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId);
        Task<Doctor> CreateAsync(Doctor doctor);
        Task<Doctor> UpdateAsync(Doctor doctor);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsByDocumentAsync(string document);
        Task<bool> ExistsByNameAndSpecialtyAsync(string fullName, int specialtyId);
    }
}
