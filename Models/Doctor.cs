using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Doctor
    {
        public long Id { get; set; }
        public string Document { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int SpecialtyId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation
        public virtual Specialty Specialty { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
