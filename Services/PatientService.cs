using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;
using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public class PatientService : IPatientService
    {
        private readonly HospitalDbContext _context;

        public PatientService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByIdAsync(long id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient?> GetByDocumentAsync(string document)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Document == document);
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToListAsync();
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            try
            {
                // Validar que no exista un paciente con el mismo documento
                if (await ExistsByDocumentAsync(patient.Document))
                {
                    throw new InvalidOperationException($"Ya existe un paciente con el documento: {patient.Document}");
                }

                // Do not set CreatedAt and UpdatedAt manually - DB handles them automatically
                // patient.CreatedAt = DateTime.UtcNow;
                // patient.UpdatedAt = DateTime.UtcNow;

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return patient;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al registrar paciente: {ex.Message}", ex);
            }
        }

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            try
            {
                var existingPatient = await GetByIdAsync(patient.Id);
                if (existingPatient == null)
                {
                    throw new InvalidOperationException($"No se encontró el paciente con ID: {patient.Id}");
                }

                // Validar que no exista otro paciente con el mismo documento
                var patientWithSameDocument = await GetByDocumentAsync(patient.Document);
                if (patientWithSameDocument != null && patientWithSameDocument.Id != patient.Id)
                {
                    throw new InvalidOperationException($"Ya existe otro paciente con el documento: {patient.Document}");
                }

                existingPatient.Document = patient.Document;
                existingPatient.FirstName = patient.FirstName;
                existingPatient.LastName = patient.LastName;
                existingPatient.Age = patient.Age;
                existingPatient.Phone = patient.Phone;
                existingPatient.Email = patient.Email;
                // Do not set UpdatedAt manually - DB handles it automatically

                await _context.SaveChangesAsync();
                return existingPatient;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar paciente: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return false;
            }

            // Verificar si tiene citas programadas
            var hasAppointments = await _context.Appointments
                .AnyAsync(a => a.PatientId == id && a.Status == AppointmentStatus.Programada);

            if (hasAppointments)
            {
                throw new InvalidOperationException("No se puede eliminar un paciente que tiene citas programadas.");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByDocumentAsync(string document)
        {
            return await _context.Patients.AnyAsync(p => p.Document == document);
        }
    }
}
