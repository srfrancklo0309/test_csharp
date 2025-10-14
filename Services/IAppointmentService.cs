using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public interface IAppointmentService
    {
        Task<Appointment?> GetByIdAsync(long id);
        Task<List<Appointment>> GetAllAsync();
        Task<List<Appointment>> GetByPatientIdAsync(long patientId);
        Task<List<Appointment>> GetByDoctorIdAsync(long doctorId);
        Task<List<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Appointment> CreateAsync(Appointment appointment);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<Appointment> CancelAsync(long id);
        Task<Appointment> MarkAsAttendedAsync(long id);
        Task<bool> DeleteAsync(long id);
        Task<bool> IsDoctorAvailableAsync(long doctorId, DateTime appointmentTime);
        Task<bool> IsPatientAvailableAsync(long patientId, DateTime appointmentTime);
    }
}
