using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;
using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly HospitalDbContext _context;

        public DoctorService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetByIdAsync(long id)
        {
            return await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor?> GetByDocumentAsync(string document)
        {
            return await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Document == document);
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Specialty)
                .Where(d => d.Active)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId)
        {
            return await _context.Doctors
                .Include(d => d.Specialty)
                .Where(d => d.SpecialtyId == specialtyId && d.Active)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<Doctor> CreateAsync(Doctor doctor)
        {
            // Validate that no doctor exists with the same document
            if (await ExistsByDocumentAsync(doctor.Document))
            {
                throw new InvalidOperationException($"Ya existe un médico con el documento: {doctor.Document}");
            }

            // Validate that no doctor exists with the same name + specialty combination
            if (await ExistsByNameAndSpecialtyAsync(doctor.FullName, doctor.SpecialtyId))
            {
                throw new InvalidOperationException($"Ya existe un médico con el nombre '{doctor.FullName}' en la especialidad seleccionada.");
            }

            // Validar que la especialidad existe
            var specialty = await _context.Specialties.FindAsync(doctor.SpecialtyId);
            if (specialty == null)
            {
                throw new InvalidOperationException($"No se encontró la especialidad con ID: {doctor.SpecialtyId}");
            }

            // Do not set CreatedAt and UpdatedAt manually - DB handles them automatically

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor> UpdateAsync(Doctor doctor)
        {
            var existingDoctor = await GetByIdAsync(doctor.Id);
            if (existingDoctor == null)
            {
                throw new InvalidOperationException($"No se encontró el médico con ID: {doctor.Id}");
            }

            // Validate that no other doctor exists with the same document
            var doctorWithSameDocument = await GetByDocumentAsync(doctor.Document);
            if (doctorWithSameDocument != null && doctorWithSameDocument.Id != doctor.Id)
            {
                throw new InvalidOperationException($"Ya existe otro médico con el documento: {doctor.Document}");
            }

            // Validate that no other doctor exists with the same name + specialty combination
            var doctorWithSameNameAndSpecialty = await _context.Doctors
                .FirstOrDefaultAsync(d => d.FullName == doctor.FullName && 
                                        d.SpecialtyId == doctor.SpecialtyId && 
                                        d.Id != doctor.Id);
            if (doctorWithSameNameAndSpecialty != null)
            {
                throw new InvalidOperationException($"Ya existe otro médico con el nombre '{doctor.FullName}' en la especialidad seleccionada.");
            }

            // Validar que la especialidad existe
            var specialty = await _context.Specialties.FindAsync(doctor.SpecialtyId);
            if (specialty == null)
            {
                throw new InvalidOperationException($"No se encontró la especialidad con ID: {doctor.SpecialtyId}");
            }

            existingDoctor.Document = doctor.Document;
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.SpecialtyId = doctor.SpecialtyId;
            existingDoctor.Phone = doctor.Phone;
            existingDoctor.Email = doctor.Email;
            existingDoctor.Active = doctor.Active;
            // Do not set UpdatedAt manually - DB handles it automatically

            await _context.SaveChangesAsync();
            return existingDoctor;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return false;
            }

            // Verificar si tiene citas programadas
            var hasAppointments = await _context.Appointments
                .AnyAsync(a => a.DoctorId == id && a.Status == AppointmentStatus.Programada);

            if (hasAppointments)
            {
                throw new InvalidOperationException("No se puede eliminar un médico que tiene citas programadas.");
            }

            // En lugar de eliminar, marcar como inactivo
            doctor.Active = false;
            doctor.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByDocumentAsync(string document)
        {
            return await _context.Doctors.AnyAsync(d => d.Document == document);
        }

        public async Task<bool> ExistsByNameAndSpecialtyAsync(string fullName, int specialtyId)
        {
            return await _context.Doctors.AnyAsync(d => d.FullName == fullName && d.SpecialtyId == specialtyId);
        }
    }
}
