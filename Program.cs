using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;
using HospitalManagement.Services;
using HospitalManagement.UI;

namespace HospitalManagement
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Hospital Management System - San Vicente Hospital ===");
            
            // Configure database connection
            var connectionString = "Server=168.119.183.3;Port=3307;Database=h_sanvicente_epm;Uid=root;Pwd=g0tIFJEQsKHm5$34Pxu1;";
            
            var optionsBuilder = new DbContextOptionsBuilder<HospitalDbContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
            
            // Verify database connection
            using var context = new HospitalDbContext(optionsBuilder.Options);
            try
            {
                await context.Database.CanConnectAsync();
                Console.WriteLine("✓ Database connection established successfully.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error connecting to database: {ex.Message}");
                Console.WriteLine("Make sure MySQL is running and the connection string is correct.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            // Create services directly
            var patientService = new PatientService(context);
            var doctorService = new DoctorService(context);
            var specialtyService = new SpecialtyService(context);
            var appointmentService = new AppointmentService(context);
            var emailService = new EmailService(context);
            
            // Create simplified UI
            var consoleUI = new ConsoleUI(
                patientService, 
                doctorService, 
                specialtyService, 
                appointmentService, 
                emailService
            );

            // Run the application
            await consoleUI.RunAsync();
        }
    }
}
