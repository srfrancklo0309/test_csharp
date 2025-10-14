using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;
using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly HospitalDbContext _context;

        public SpecialtyService(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Specialty>> GetAllAsync()
        {
            return await _context.Specialties
                .Include(s => s.Doctors)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Specialty?> GetByIdAsync(int id)
        {
            return await _context.Specialties
                .Include(s => s.Doctors)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Specialty> CreateAsync(Specialty specialty)
        {
            // Validar que no exista una especialidad con el mismo nombre
            var existingSpecialty = await _context.Specialties
                .FirstOrDefaultAsync(s => s.Name.ToLower() == specialty.Name.ToLower());

            if (existingSpecialty != null)
            {
                throw new InvalidOperationException($"Ya existe una especialidad con el nombre: {specialty.Name}");
            }

            _context.Specialties.Add(specialty);
            await _context.SaveChangesAsync();
            return specialty;
        }

        public async Task<Specialty> UpdateAsync(Specialty specialty)
        {
            var existingSpecialty = await GetByIdAsync(specialty.Id);
            if (existingSpecialty == null)
            {
                throw new InvalidOperationException($"No se encontró la especialidad con ID: {specialty.Id}");
            }

            // Validar que no exista otra especialidad con el mismo nombre
            var specialtyWithSameName = await _context.Specialties
                .FirstOrDefaultAsync(s => s.Name.ToLower() == specialty.Name.ToLower() && s.Id != specialty.Id);

            if (specialtyWithSameName != null)
            {
                throw new InvalidOperationException($"Ya existe otra especialidad con el nombre: {specialty.Name}");
            }

            existingSpecialty.Name = specialty.Name;
            await _context.SaveChangesAsync();
            return existingSpecialty;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return false;
            }

            // Check if there are doctors associated with this specialty
            var hasDoctors = await _context.Doctors.AnyAsync(d => d.SpecialtyId == id);
            if (hasDoctors)
            {
                throw new InvalidOperationException("No se puede eliminar una especialidad que tiene médicos asociados.");
            }

            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
