using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public interface IPatientService
    {
        Task<Patient?> GetByIdAsync(long id);
        Task<Patient?> GetByDocumentAsync(string document);
        Task<List<Patient>> GetAllAsync();
        Task<Patient> CreateAsync(Patient patient);
        Task<Patient> UpdateAsync(Patient patient);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsByDocumentAsync(string document);
    }
}
