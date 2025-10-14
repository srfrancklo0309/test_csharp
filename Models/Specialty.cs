using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Navigation
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
