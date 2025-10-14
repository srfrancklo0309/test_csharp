using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;
using HospitalManagement.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text.Json;

namespace HospitalManagement.Services
{
    public class EmailService : IEmailService
    {
        private readonly HospitalDbContext _context;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly bool _enableSsl;
        private readonly bool _useAuthentication;

        public EmailService(HospitalDbContext context)
        {
            _context = context;
            
            // Load configuration from appsettings.json
            var config = LoadEmailConfiguration();
            
            _smtpServer = config.SmtpServer;
            _smtpPort = config.SmtpPort;
            _smtpUsername = config.SmtpUsername;
            _smtpPassword = config.SmtpPassword;
            _fromEmail = config.FromEmail;
            _fromName = config.FromName;
            _enableSsl = config.EnableSsl;
            _useAuthentication = config.UseAuthentication;
        }

        private (string SmtpServer, int SmtpPort, string SmtpUsername, string SmtpPassword, 
                string FromEmail, string FromName, bool EnableSsl, bool UseAuthentication) LoadEmailConfiguration()
        {
            try
            {
                var jsonString = File.ReadAllText("appsettings.json");
                var config = JsonSerializer.Deserialize<JsonElement>(jsonString);
                
                var emailSettings = config.GetProperty("EmailSettings");
                
                return (
                    SmtpServer: emailSettings.GetProperty("SmtpServer").GetString() ?? "smtp.gmail.com",
                    SmtpPort: emailSettings.GetProperty("SmtpPort").GetInt32(),
                    SmtpUsername: emailSettings.GetProperty("SmtpUsername").GetString() ?? "",
                    SmtpPassword: emailSettings.GetProperty("SmtpPassword").GetString() ?? "",
                    FromEmail: emailSettings.GetProperty("FromEmail").GetString() ?? "",
                    FromName: emailSettings.GetProperty("FromName").GetString() ?? "Hospital San Vicente",
                    EnableSsl: emailSettings.GetProperty("EnableSsl").GetBoolean(),
                    UseAuthentication: emailSettings.GetProperty("UseAuthentication").GetBoolean()
                );
            }
            catch (Exception ex)
            {
                // Default configuration if there's an error loading
                Console.WriteLine($"⚠️ Warning: Could not load email configuration: {ex.Message}");
                Console.WriteLine("📝 Using default configuration...");
                
                return (
                    SmtpServer: "smtp.gmail.com",
                    SmtpPort: 587,
                    SmtpUsername: "tu-email@gmail.com",
                    SmtpPassword: "tu-app-password",
                    FromEmail: "tu-email@gmail.com",
                    FromName: "Hospital San Vicente",
                    EnableSsl: true,
                    UseAuthentication: true
                );
            }
        }

        public async Task<EmailLog> SendAppointmentConfirmationAsync(Appointment appointment)
        {
            var appointmentWithDetails = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Specialty)
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            if (appointmentWithDetails == null)
            {
                throw new InvalidOperationException($"No se encontró la cita con ID: {appointment.Id}");
            }

            if (string.IsNullOrEmpty(appointmentWithDetails.Patient.Email))
            {
                throw new InvalidOperationException("El paciente no tiene un email registrado");
            }

            var emailLog = new EmailLog
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                ToEmail = appointmentWithDetails.Patient.Email,
                Subject = "Confirmación de Cita Médica - Hospital San Vicente",
                Body = GenerateAppointmentEmailBody(appointmentWithDetails),
                Status = EmailStatus.NoEnviado,
                SentAt = DateTime.UtcNow
            };

            try
            {
                await SendEmailAsync(emailLog.ToEmail, emailLog.Subject, emailLog.Body);
                emailLog.Status = EmailStatus.Enviado;
            }
            catch (Exception ex)
            {
                emailLog.Status = EmailStatus.Error;
                emailLog.ErrorMessage = ex.Message;
            }

            _context.EmailLogs.Add(emailLog);
            await _context.SaveChangesAsync();

            return emailLog;
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            try
            {
                // Configure security options
                var secureSocketOptions = _enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
                
                await client.ConnectAsync(_smtpServer, _smtpPort, secureSocketOptions);
                
                // Authentication only if enabled
                if (_useAuthentication && !string.IsNullOrEmpty(_smtpUsername) && !string.IsNullOrEmpty(_smtpPassword))
                {
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                }
                
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al enviar email: {ex.Message}", ex);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        private string GenerateAppointmentEmailBody(Appointment appointment)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #2c5aa0; text-align: center;'>Hospital San Vicente</h2>
                        <h3 style='color: #2c5aa0;'>Confirmación de Cita Médica</h3>
                        
                        <p>Estimado/a <strong>{appointment.Patient.FullName}</strong>,</p>
                        
                        <p>Le confirmamos que su cita médica ha sido programada exitosamente:</p>
                        
                        <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p><strong>Fecha y Hora:</strong> {appointment.AppointmentAt:dddd, dd 'de' MMMM 'de' yyyy 'a las' HH:mm}</p>
                            <p><strong>Médico:</strong> Dr(a). {appointment.Doctor.FullName}</p>
                            <p><strong>Especialidad:</strong> {appointment.Doctor.Specialty.Name}</p>
                            <p><strong>Motivo:</strong> {appointment.Reason ?? "No especificado"}</p>
                        </div>
                        
                        <p><strong>Instrucciones importantes:</strong></p>
                        <ul>
                            <li>Llegue 15 minutos antes de su cita</li>
                            <li>Traiga su documento de identidad</li>
                            <li>Si necesita cancelar o reprogramar, contáctenos con al menos 24 horas de anticipación</li>
                        </ul>
                        
                        <p>Si tiene alguna pregunta, no dude en contactarnos.</p>
                        
                        <p>Saludos cordiales,<br>
                        <strong>Hospital San Vicente</strong></p>
                        
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='font-size: 12px; color: #666; text-align: center;'>
                            Este es un mensaje automático, por favor no responda a este correo.
                        </p>
                    </div>
                </body>
                </html>";
        }

        public async Task<List<EmailLog>> GetEmailLogsByAppointmentIdAsync(long appointmentId)
        {
            return await _context.EmailLogs
                .Include(e => e.Appointment)
                .Include(e => e.Patient)
                .Where(e => e.AppointmentId == appointmentId)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }

        public async Task<List<EmailLog>> GetEmailLogsByPatientIdAsync(long patientId)
        {
            return await _context.EmailLogs
                .Include(e => e.Appointment)
                .Include(e => e.Patient)
                .Where(e => e.PatientId == patientId)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }

        public async Task<List<EmailLog>> GetAllEmailLogsAsync()
        {
            return await _context.EmailLogs
                .Include(e => e.Appointment)
                .Include(e => e.Patient)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }
    }
}
