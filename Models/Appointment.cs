using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public long DoctorId { get; set; }
        public DateTime AppointmentAt { get; set; }
        public string? Reason { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Programada;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation
        public virtual Patient Patient { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();
    }
}
