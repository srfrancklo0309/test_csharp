using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public interface IEmailService
    {
        Task<EmailLog> SendAppointmentConfirmationAsync(Appointment appointment);
        Task<List<EmailLog>> GetEmailLogsByAppointmentIdAsync(long appointmentId);
        Task<List<EmailLog>> GetEmailLogsByPatientIdAsync(long patientId);
        Task<List<EmailLog>> GetAllEmailLogsAsync();
    }
}
