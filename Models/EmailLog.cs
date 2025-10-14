using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class EmailLog
    {
        public long Id { get; set; }
        public long AppointmentId { get; set; }
        public long PatientId { get; set; }
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? Body { get; set; }
        public EmailStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        
        // Navigation
        public virtual Appointment Appointment { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
    }
}
