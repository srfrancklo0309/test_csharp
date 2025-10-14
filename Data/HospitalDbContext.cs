using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models;

namespace HospitalManagement.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar nombres de tablas para que coincidan con la BD
            modelBuilder.Entity<Specialty>().ToTable("specialties");
            modelBuilder.Entity<Patient>().ToTable("patients");
            modelBuilder.Entity<Doctor>().ToTable("doctors");
            modelBuilder.Entity<Appointment>().ToTable("appointments");
            modelBuilder.Entity<EmailLog>().ToTable("email_log");

            // Specialty configuration
            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Document).IsRequired().HasMaxLength(30);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(30);
                entity.HasIndex(e => e.Document).IsUnique();
                
                // Mapeo de columnas con snake_case
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                // Age validation (temporarily commented for debug)
                // entity.ToTable(t => t.HasCheckConstraint("CK_Patient_Age", "Age >= 0 AND Age <= 120"));
            });

            // Doctor configuration
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Document).IsRequired().HasMaxLength(30);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(30);
                entity.HasIndex(e => e.Document).IsUnique();
                
                // Mapeo de columnas con snake_case
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                // Unicidad de nombre + especialidad
                entity.HasIndex(e => new { e.FullName, e.SpecialtyId }).IsUnique();
                
                // Relationship with Specialty
                entity.HasOne(d => d.Specialty)
                      .WithMany(s => s.Doctors)
                      .HasForeignKey(d => d.SpecialtyId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Appointment configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AppointmentAt).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>();
                
                // Mapeo de columnas con snake_case
                entity.Property(e => e.PatientId).HasColumnName("patient_id");
                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
                entity.Property(e => e.AppointmentAt).HasColumnName("appointment_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                
                // Uniqueness of time slot per doctor
                entity.HasIndex(e => new { e.DoctorId, e.AppointmentAt }).IsUnique();
                
                // Uniqueness of time slot per patient
                entity.HasIndex(e => new { e.PatientId, e.AppointmentAt }).IsUnique();
                
                // Relationships
                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // EmailLog configuration
            modelBuilder.Entity<EmailLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ToEmail).IsRequired();
                entity.Property(e => e.Subject).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>();
                
                // Mapeo de columnas con snake_case
                entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
                entity.Property(e => e.PatientId).HasColumnName("patient_id");
                entity.Property(e => e.ToEmail).HasColumnName("to_email");
                entity.Property(e => e.ErrorMessage).HasColumnName("error_message");
                entity.Property(e => e.SentAt).HasColumnName("sent_at");
                
                // Relaciones
                entity.HasOne(e => e.Appointment)
                      .WithMany(a => a.EmailLogs)
                      .HasForeignKey(e => e.AppointmentId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.EmailLogs)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Datos iniciales
            modelBuilder.Entity<Specialty>().HasData(
                new Specialty { Id = 1, Name = "Medicina General" },
                new Specialty { Id = 2, Name = "Pediatría" },
                new Specialty { Id = 3, Name = "Cardiología" },
                new Specialty { Id = 4, Name = "Dermatología" },
                new Specialty { Id = 5, Name = "Ginecología" },
                new Specialty { Id = 6, Name = "Neurología" },
                new Specialty { Id = 7, Name = "Ortopedia" }
            );
        }
    }
}
