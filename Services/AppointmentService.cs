using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;
using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HospitalDbContext _context;

        public AppointmentService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdAsync(long id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Specialty)
                .Include(a => a.EmailLogs)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Specialty)
                .OrderBy(a => a.AppointmentAt)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(long patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Specialty)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentAt)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(long doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Specialty)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentAt)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Specialty)
                .Where(a => a.AppointmentAt >= startDate && a.AppointmentAt <= endDate)
                .OrderBy(a => a.AppointmentAt)
                .ToListAsync();
        }

        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            // Validar que el paciente existe
            var patient = await _context.Patients.FindAsync(appointment.PatientId);
            if (patient == null)
            {
                throw new InvalidOperationException($"No se encontró el paciente con ID: {appointment.PatientId}");
            }

            // Validate that the doctor exists and is active
            var doctor = await _context.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == appointment.DoctorId && d.Active);
            if (doctor == null)
            {
                throw new InvalidOperationException($"No se encontró el médico con ID: {appointment.DoctorId} o no está activo");
            }

            // Validate that the doctor is available at that time
            if (!await IsDoctorAvailableAsync(appointment.DoctorId, appointment.AppointmentAt))
            {
                throw new InvalidOperationException($"El médico no está disponible en el horario: {appointment.AppointmentAt:dd/MM/yyyy HH:mm}");
            }

            // Validate that the patient is available at that time
            if (!await IsPatientAvailableAsync(appointment.PatientId, appointment.AppointmentAt))
            {
                throw new InvalidOperationException($"El paciente ya tiene una cita programada en el horario: {appointment.AppointmentAt:dd/MM/yyyy HH:mm}");
            }

            // Validar que la fecha no sea en el pasado
            if (appointment.AppointmentAt <= DateTime.Now)
            {
                throw new InvalidOperationException("No se puede agendar una cita en el pasado");
            }

            appointment.CreatedAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.Status = AppointmentStatus.Programada;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await GetByIdAsync(appointment.Id);
            if (existingAppointment == null)
            {
                throw new InvalidOperationException($"No se encontró la cita con ID: {appointment.Id}");
            }

            // If changing the time, validate availability
            if (existingAppointment.AppointmentAt != appointment.AppointmentAt)
            {
                if (!await IsDoctorAvailableAsync(appointment.DoctorId, appointment.AppointmentAt))
                {
                    throw new InvalidOperationException($"El médico no está disponible en el horario: {appointment.AppointmentAt:dd/MM/yyyy HH:mm}");
                }

                if (!await IsPatientAvailableAsync(appointment.PatientId, appointment.AppointmentAt))
                {
                    throw new InvalidOperationException($"El paciente ya tiene una cita programada en el horario: {appointment.AppointmentAt:dd/MM/yyyy HH:mm}");
                }
            }

            existingAppointment.PatientId = appointment.PatientId;
            existingAppointment.DoctorId = appointment.DoctorId;
            existingAppointment.AppointmentAt = appointment.AppointmentAt;
            existingAppointment.Reason = appointment.Reason;
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingAppointment;
        }

        public async Task<Appointment> CancelAsync(long id)
        {
            var appointment = await GetByIdAsync(id);
            if (appointment == null)
            {
                throw new InvalidOperationException($"No se encontró la cita con ID: {id}");
            }

            if (appointment.Status == AppointmentStatus.Cancelada)
            {
                throw new InvalidOperationException("La cita ya está cancelada");
            }

            if (appointment.Status == AppointmentStatus.Atendida)
            {
                throw new InvalidOperationException("No se puede cancelar una cita que ya fue atendida");
            }

            appointment.Status = AppointmentStatus.Cancelada;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> MarkAsAttendedAsync(long id)
        {
            var appointment = await GetByIdAsync(id);
            if (appointment == null)
            {
                throw new InvalidOperationException($"No se encontró la cita con ID: {id}");
            }

            if (appointment.Status == AppointmentStatus.Atendida)
            {
                throw new InvalidOperationException("La cita ya fue marcada como atendida");
            }

            if (appointment.Status == AppointmentStatus.Cancelada)
            {
                throw new InvalidOperationException("No se puede marcar como atendida una cita cancelada");
            }

            appointment.Status = AppointmentStatus.Atendida;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return false;
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDoctorAvailableAsync(long doctorId, DateTime appointmentTime)
        {
            return !await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && 
                              a.AppointmentAt == appointmentTime && 
                              a.Status == AppointmentStatus.Programada);
        }

        public async Task<bool> IsPatientAvailableAsync(long patientId, DateTime appointmentTime)
        {
            return !await _context.Appointments
                .AnyAsync(a => a.PatientId == patientId && 
                              a.AppointmentAt == appointmentTime && 
                              a.Status == AppointmentStatus.Programada);
        }
    }
}
