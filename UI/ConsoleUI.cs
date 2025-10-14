using HospitalManagement.Services;
using HospitalManagement.Models;

namespace HospitalManagement.UI
{
    public class ConsoleUI
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IAppointmentService _appointmentService;
        private readonly IEmailService _emailService;

        public ConsoleUI(
            IPatientService patientService,
            IDoctorService doctorService,
            ISpecialtyService specialtyService,
            IAppointmentService appointmentService,
            IEmailService emailService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _specialtyService = specialtyService;
            _appointmentService = appointmentService;
            _emailService = emailService;
        }

        public async Task RunAsync()
        {
            bool continuar = true;
            while (continuar)
            {
                try
                {
                    MostrarMenuPrincipal();
                    var opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            await GestionarPacientes();
                            break;
                        case "2":
                            await GestionarMedicos();
                            break;
                        case "3":
                            await GestionarCitas();
                            break;
                        case "4":
                            await GestionarEspecialidades();
                            break;
                        case "5":
                            await VerHistorialCorreos();
                            break;
                        case "0":
                            continuar = false;
                            Console.WriteLine("¡Gracias por usar el sistema del Hospital San Vicente!");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente nuevamente.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (continuar)
                {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
                }
            }
        }

        private void MostrarMenuPrincipal()
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. Patient Management");
            Console.WriteLine("2. Doctor Management");
            Console.WriteLine("3. Medical Appointment Management");
            Console.WriteLine("4. Specialty Management");
            Console.WriteLine("5. Email History");
            Console.WriteLine("0. Exit");
            Console.Write("Seleccione una opción: ");
        }

        private async Task GestionarPacientes()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n=== PATIENT MANAGEMENT ===");
                Console.WriteLine("1. Register new patient");
                Console.WriteLine("2. Edit patient");
                Console.WriteLine("3. List all patients");
                Console.WriteLine("4. Search patient by document");
                Console.WriteLine("0. Return to main menu");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        await RegistrarPaciente();
                        break;
                    case "2":
                        await EditarPaciente();
                        break;
                    case "3":
                        await ListarPacientes();
                        break;
                    case "4":
                        await BuscarPacientePorDocumento();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        private async Task GestionarMedicos()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n=== DOCTOR MANAGEMENT ===");
                Console.WriteLine("1. Register new doctor");
                Console.WriteLine("2. Edit doctor");
                Console.WriteLine("3. List all doctors");
                Console.WriteLine("4. List doctors by specialty");
                Console.WriteLine("0. Return to main menu");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        await RegistrarMedico();
                        break;
                    case "2":
                        await EditarMedico();
                        break;
                    case "3":
                        await ListarMedicos();
                        break;
                    case "4":
                        await ListarMedicosPorEspecialidad();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        private async Task GestionarCitas()
        {
            bool continuar = true;
            while (continuar)
            {
            Console.WriteLine("\n=== MEDICAL APPOINTMENT MANAGEMENT ===");
                Console.WriteLine("1. Schedule new appointment");
                Console.WriteLine("2. Cancel appointment");
                Console.WriteLine("3. Mark appointment as attended");
                Console.WriteLine("4. List appointments by patient");
                Console.WriteLine("5. List appointments by doctor");
                Console.WriteLine("6. List all appointments");
                Console.WriteLine("0. Return to main menu");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        await AgendarCita();
                        break;
                    case "2":
                        await CancelarCita();
                        break;
                    case "3":
                        await MarcarCitaAtendida();
                        break;
                    case "4":
                        await ListarCitasPorPaciente();
                        break;
                    case "5":
                        await ListarCitasPorMedico();
                        break;
                    case "6":
                        await ListarTodasLasCitas();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        private async Task GestionarEspecialidades()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n=== SPECIALTY MANAGEMENT ===");
                Console.WriteLine("1. List all specialties");
                Console.WriteLine("0. Return to main menu");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        await ListarEspecialidades();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        // Simplified implementation methods
        private async Task RegistrarPaciente()
        {
            Console.WriteLine("\n=== REGISTRAR PACIENTE ===");
            Console.Write("Documento: ");
            var documento = Console.ReadLine() ?? "";
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine() ?? "";
            Console.Write("Apellido: ");
            var apellido = Console.ReadLine() ?? "";
            Console.Write("Edad: ");
            var edadStr = Console.ReadLine() ?? "";
            Console.Write("Teléfono: ");
            var telefono = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(documento))
            {
                Console.WriteLine("❌ Error: El documento es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("❌ Error: El nombre es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Console.WriteLine("❌ Error: El apellido es obligatorio.");
                return;
            }

            if (int.TryParse(edadStr, out int edad))
            {
                // Validar rango de edad
                if (edad < 0 || edad > 120)
                {
                    Console.WriteLine("❌ Error: La edad debe estar entre 0 y 120 años.");
                    Console.WriteLine($"💡 Usted ingresó: {edad} años");
                    return;
                }

                // Validar formato de email si se proporciona
                if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
                {
                    Console.WriteLine("❌ Error: El formato del email no es válido.");
                    Console.WriteLine("💡 Sugerencia: Use un formato como ejemplo@correo.com");
                    return;
                }

                try
                {
                    var paciente = new Patient
                    {
                        Document = documento,
                        FirstName = nombre,
                        LastName = apellido,
                        Age = edad,
                        Phone = telefono,
                        Email = email
                    };

                    await _patientService.CreateAsync(paciente);
                    Console.WriteLine("✅ Paciente registrado exitosamente.");
                    Console.WriteLine($"👤 Nombre: {nombre} {apellido}");
                    Console.WriteLine($"🆔 Documento: {documento}");
                    Console.WriteLine($"🎂 Edad: {edad} años");
                    if (!string.IsNullOrWhiteSpace(telefono))
                        Console.WriteLine($"📞 Teléfono: {telefono}");
                    if (!string.IsNullOrWhiteSpace(email))
                        Console.WriteLine($"📧 Email: {email}");
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message.Contains("Ya existe un paciente con el documento"))
                    {
                        Console.WriteLine($"❌ Error: Ya existe un paciente registrado con el documento {documento}.");
                        Console.WriteLine("💡 Sugerencia: Verifique el número de documento o consulte la lista de pacientes.");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error inesperado: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Detalle técnico: {ex.InnerException.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Error: La edad debe ser un número válido.");
                Console.WriteLine($"💡 Usted ingresó: '{edadStr}'");
                Console.WriteLine("📝 Sugerencia: Ingrese solo números (ejemplo: 25)");
            }
        }

        private async Task EditarPaciente()
        {
            Console.WriteLine("\n=== EDITAR PACIENTE ===");
            Console.Write("Documento del paciente a editar: ");
            var documento = Console.ReadLine() ?? "";
            
            try
            {
                var paciente = await _patientService.GetByDocumentAsync(documento);
                if (paciente == null)
                {
                    Console.WriteLine("Paciente no encontrado.");
                return;
            }

                Console.WriteLine($"Editando: {paciente.FirstName} {paciente.LastName}");
                Console.Write("Nuevo nombre (Enter para mantener actual): ");
                var nuevoNombre = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoNombre))
                    paciente.FirstName = nuevoNombre;

                Console.Write("Nuevo apellido (Enter para mantener actual): ");
                var nuevoApellido = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoApellido))
                    paciente.LastName = nuevoApellido;

                Console.Write("Nueva edad (Enter para mantener actual): ");
                var nuevaEdadStr = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevaEdadStr) && int.TryParse(nuevaEdadStr, out int nuevaEdad))
                    paciente.Age = nuevaEdad;

                Console.Write("Nuevo teléfono (Enter para mantener actual): ");
                var nuevoTelefono = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoTelefono))
                    paciente.Phone = nuevoTelefono;

                Console.Write("Nuevo email (Enter para mantener actual): ");
                var nuevoEmail = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoEmail))
                    paciente.Email = nuevoEmail;

                await _patientService.UpdateAsync(paciente);
                Console.WriteLine("✓ Paciente actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalle: {ex.InnerException.Message}");
                }
            }
        }

        private async Task ListarPacientes()
        {
            Console.WriteLine("\n=== LISTA DE PACIENTES ===");
            try
            {
                var pacientes = await _patientService.GetAllAsync();
                if (pacientes.Any())
                {
                    foreach (var paciente in pacientes)
                    {
                        Console.WriteLine($"• {paciente.FirstName} {paciente.LastName} - Doc: {paciente.Document} - Edad: {paciente.Age}");
                    }
                }
                else
                {
                    Console.WriteLine("No hay pacientes registrados.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task BuscarPacientePorDocumento()
        {
            Console.WriteLine("\n=== BUSCAR PACIENTE ===");
            Console.Write("Documento: ");
            var documento = Console.ReadLine() ?? "";

            try
            {
                var paciente = await _patientService.GetByDocumentAsync(documento);
                if (paciente != null)
                {
                    Console.WriteLine($"Nombre: {paciente.FirstName} {paciente.LastName}");
                    Console.WriteLine($"Documento: {paciente.Document}");
                    Console.WriteLine($"Edad: {paciente.Age}");
                    Console.WriteLine($"Teléfono: {paciente.Phone}");
                    Console.WriteLine($"Email: {paciente.Email}");
                }
                else
                {
                    Console.WriteLine("Paciente no encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task RegistrarMedico()
        {
            Console.WriteLine("\n=== REGISTRAR MÉDICO ===");
            Console.Write("Documento: ");
            var documento = Console.ReadLine() ?? "";
            Console.Write("Nombre completo: ");
            var nombreCompleto = Console.ReadLine() ?? "";
            Console.Write("Teléfono: ");
            var telefono = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";

            // Listar especialidades disponibles
            await ListarEspecialidades();
            Console.Write("ID de especialidad: ");
            var especialidadIdStr = Console.ReadLine() ?? "";

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(documento))
            {
                Console.WriteLine("❌ Error: El documento es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                Console.WriteLine("❌ Error: El nombre completo es obligatorio.");
                return;
            }

            // Validar formato de email si se proporciona
            if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
            {
                Console.WriteLine("❌ Error: El formato del email no es válido.");
                Console.WriteLine("💡 Sugerencia: Use un formato como ejemplo@correo.com");
                return;
            }

            if (int.TryParse(especialidadIdStr, out int especialidadId))
            {
                try
                {
                    var medico = new Doctor
                    {
                        Document = documento,
                        FullName = nombreCompleto,
                        Phone = telefono,
                        Email = email,
                        SpecialtyId = especialidadId,
                        Active = true
                    };

                    await _doctorService.CreateAsync(medico);
                    Console.WriteLine("✅ Médico registrado exitosamente.");
                    Console.WriteLine($"👨‍⚕️ Nombre: Dr(a). {nombreCompleto}");
                    Console.WriteLine($"🆔 Documento: {documento}");
                    if (!string.IsNullOrWhiteSpace(telefono))
                        Console.WriteLine($"📞 Teléfono: {telefono}");
                    if (!string.IsNullOrWhiteSpace(email))
                        Console.WriteLine($"📧 Email: {email}");
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message.Contains("Ya existe un médico con el documento"))
                    {
                        Console.WriteLine($"❌ Error: Ya existe un médico registrado con el documento {documento}.");
                        Console.WriteLine("💡 Sugerencia: Verifique el número de documento o consulte la lista de médicos.");
                    }
                    else if (ex.Message.Contains("Ya existe un médico con el nombre"))
                    {
                        Console.WriteLine($"❌ Error: Ya existe un médico con el nombre '{nombreCompleto}' en la especialidad seleccionada.");
                        Console.WriteLine("💡 Sugerencia: Elija otro nombre o verifique la especialidad.");
                    }
                    else if (ex.Message.Contains("No se encontró la especialidad"))
                    {
                        Console.WriteLine($"❌ Error: La especialidad con ID {especialidadId} no existe.");
                        Console.WriteLine("💡 Sugerencia: Consulte la lista de especialidades disponibles.");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                    Console.WriteLine($"❌ Error inesperado: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Detalle técnico: {ex.InnerException.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Error: ID de especialidad debe ser un número válido.");
                Console.WriteLine($"💡 Usted ingresó: '{especialidadIdStr}'");
                Console.WriteLine("📝 Sugerencia: Ingrese solo números (ejemplo: 1, 2, 3...)");
            }
        }

        private async Task EditarMedico()
        {
            Console.WriteLine("\n=== EDITAR MÉDICO ===");
            Console.Write("Documento del médico a editar: ");
            var documento = Console.ReadLine() ?? "";
            
            try
            {
                var medico = await _doctorService.GetByDocumentAsync(documento);
                if (medico == null)
                {
                    Console.WriteLine("Médico no encontrado.");
                    return;
                }

                Console.WriteLine($"Editando: {medico.FullName}");
                Console.Write("Nuevo nombre completo (Enter para mantener actual): ");
                var nuevoNombre = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoNombre))
                    medico.FullName = nuevoNombre;

                Console.Write("Nuevo teléfono (Enter para mantener actual): ");
                var nuevoTelefono = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoTelefono))
                    medico.Phone = nuevoTelefono;

                Console.Write("Nuevo email (Enter para mantener actual): ");
                var nuevoEmail = Console.ReadLine() ?? "";
                if (!string.IsNullOrEmpty(nuevoEmail))
                    medico.Email = nuevoEmail;

                await _doctorService.UpdateAsync(medico);
                Console.WriteLine("✓ Médico actualizado exitosamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Detalle: {ex.InnerException.Message}");
                    }
                }
        }

        private async Task ListarMedicos()
        {
            Console.WriteLine("\n=== LISTA DE MÉDICOS ===");
            try
            {
                var medicos = await _doctorService.GetAllAsync();
                if (medicos.Any())
                {
                    foreach (var medico in medicos)
                    {
                        var especialidad = await _specialtyService.GetByIdAsync(medico.SpecialtyId);
                        Console.WriteLine($"• Dr(a). {medico.FullName} - Doc: {medico.Document} - Especialidad: {especialidad?.Name}");
                }
            }
            else
            {
                    Console.WriteLine("No hay médicos registrados.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task ListarMedicosPorEspecialidad()
        {
            Console.WriteLine("\n=== MÉDICOS POR ESPECIALIDAD ===");
            await ListarEspecialidades();
            Console.Write("ID de especialidad: ");
            var especialidadIdStr = Console.ReadLine();

            if (int.TryParse(especialidadIdStr, out int especialidadId))
                {
                    try
                    {
                    var medicos = await _doctorService.GetBySpecialtyAsync(especialidadId);
                    if (medicos.Any())
                    {
                        foreach (var medico in medicos)
                        {
                            Console.WriteLine($"• Dr(a). {medico.FullName} - Doc: {medico.Document}");
                    }
                }
                else
                {
                        Console.WriteLine("No hay médicos registrados en esta especialidad.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Error: ID de especialidad debe ser un número válido.");
            }
        }

        private async Task AgendarCita()
        {
            Console.WriteLine("\n=== AGENDAR CITA ===");
            
            // Seleccionar paciente
            Console.Write("Documento del paciente: ");
            var documentoPaciente = Console.ReadLine() ?? "";
            
            try
            {
                var paciente = await _patientService.GetByDocumentAsync(documentoPaciente);
                if (paciente == null)
                {
                    Console.WriteLine("Paciente no encontrado.");
                    return;
                }

                Console.WriteLine($"Paciente: {paciente.FirstName} {paciente.LastName}");

                // Select doctor
                Console.Write("Documento del médico: ");
                var documentoMedico = Console.ReadLine() ?? "";
                
                var medico = await _doctorService.GetByDocumentAsync(documentoMedico);
                if (medico == null)
                {
                    Console.WriteLine("Médico no encontrado.");
                return;
            }

                Console.WriteLine($"Médico: Dr(a). {medico.FullName}");

                // Fecha y hora
                Console.Write("Fecha (yyyy-mm-dd): ");
                var fechaStr = Console.ReadLine() ?? "";
                Console.Write("Hora (hh:mm): ");
                var horaStr = Console.ReadLine() ?? "";

                if (DateTime.TryParse($"{fechaStr} {horaStr}", out DateTime fechaHora))
                {
                 
                    if (fechaHora < DateTime.Now)
                    {
                        Console.WriteLine("❌ Error: No se pueden agendar citas en fechas pasadas.");
                        return;
                    }

                    // Validate doctor availability
                    if (!await _appointmentService.IsDoctorAvailableAsync(medico.Id, fechaHora))
                    {
                        Console.WriteLine($"❌ Error: El Dr(a). {medico.FullName} ya tiene una cita programada para {fechaHora:dd/MM/yyyy HH:mm}.");
                        Console.WriteLine("💡 Sugerencia: Elija otro horario o consulte la disponibilidad del médico.");
                return;
            }

                  
                    if (!await _appointmentService.IsPatientAvailableAsync(paciente.Id, fechaHora))
                    {
                        Console.WriteLine($"❌ Error: El paciente {paciente.FirstName} {paciente.LastName} ya tiene una cita programada para {fechaHora:dd/MM/yyyy HH:mm}.");
                        Console.WriteLine("💡 Sugerencia: Elija otro horario para este paciente.");
                        return;
                    }

                    var cita = new Appointment
                    {
                        PatientId = paciente.Id,
                        DoctorId = medico.Id,
                        AppointmentAt = fechaHora,
                        Reason = "Consulta médica",
                        Status = AppointmentStatus.Programada
                    };

                    await _appointmentService.CreateAsync(cita);
                    Console.WriteLine("✅ Cita agendada exitosamente.");
                    Console.WriteLine($"📅 Fecha: {fechaHora:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"👤 Paciente: {paciente.FirstName} {paciente.LastName}");
                    Console.WriteLine($"👨‍⚕️ Médico: Dr(a). {medico.FullName}");
                    
                    // Send confirmation email
                    try
                    {
                        await _emailService.SendAppointmentConfirmationAsync(cita);
                        Console.WriteLine("📧 Email de confirmación enviado.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Advertencia: No se pudo enviar el email: {ex.Message}");
                }
            }
            else
            {
                    Console.WriteLine("❌ Error: Fecha u hora inválida.");
                    Console.WriteLine("💡 Sugerencia: Use el formato correcto (yyyy-mm-dd) para fecha y (hh:mm) para hora.");
                    Console.WriteLine("📝 Ejemplo: 2024-12-25 y 14:30");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Handle AppointmentService validation errors
                if (ex.Message.Contains("no está disponible"))
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    Console.WriteLine("💡 Sugerencia: Elija otro horario o consulte la disponibilidad del médico.");
                }
                else if (ex.Message.Contains("ya tiene una cita programada"))
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    Console.WriteLine("💡 Sugerencia: Elija otro horario para este paciente.");
                }
                else if (ex.Message.Contains("No se puede agendar una cita en el pasado"))
                {
                    Console.WriteLine("❌ Error: No se pueden agendar citas en fechas pasadas.");
                    Console.WriteLine("💡 Sugerencia: Elija una fecha y hora futura.");
                }
                else if (ex.Message.Contains("No se encontró el paciente"))
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    Console.WriteLine("💡 Sugerencia: Verifique el documento del paciente.");
                }
                else if (ex.Message.Contains("No se encontró el médico"))
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    Console.WriteLine("💡 Sugerencia: Verifique el documento del médico.");
            }
            else
            {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Detalle técnico: {ex.InnerException.Message}");
                }
            }
        }

        private async Task CancelarCita()
        {
            Console.WriteLine("\n=== CANCELAR CITA ===");
            Console.WriteLine("💡 Sugerencia: Use 'Listar todas las citas' para ver los IDs disponibles.");
            Console.Write("ID de la cita a cancelar: ");
            var citaIdStr = Console.ReadLine() ?? "";

            if (long.TryParse(citaIdStr, out long citaId))
            {
                try
                {
                    // Verificar que la cita existe antes de cancelar
                    var cita = await _appointmentService.GetByIdAsync(citaId);
                    if (cita == null)
                    {
                        Console.WriteLine($"❌ Error: No se encontró una cita con ID {citaId}.");
                        Console.WriteLine("💡 Sugerencia: Verifique el ID o consulte la lista de citas.");
                    return;
                }

                    if (cita.Status == AppointmentStatus.Cancelada)
                {
                        Console.WriteLine($"❌ Error: La cita con ID {citaId} ya está cancelada.");
                    return;
                }

                    if (cita.Status == AppointmentStatus.Atendida)
                    {
                        Console.WriteLine($"❌ Error: No se puede cancelar una cita que ya fue atendida.");
                        return;
                    }

                    await _appointmentService.CancelAsync(citaId);
                    Console.WriteLine("✅ Cita cancelada exitosamente.");
                    Console.WriteLine($"📅 Fecha: {cita.AppointmentAt:dd/MM/yyyy HH:mm}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Detalle: {ex.InnerException.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Error: ID de cita debe ser un número válido.");
                Console.WriteLine($"💡 Usted ingresó: '{citaIdStr}'");
                Console.WriteLine("📝 Sugerencia: Ingrese solo números (ejemplo: 123)");
            }
        }

        private async Task MarcarCitaAtendida()
        {
            Console.WriteLine("\n=== MARCAR CITA COMO ATENDIDA ===");
            Console.WriteLine("💡 Sugerencia: Use 'Listar todas las citas' para ver los IDs disponibles.");
            Console.Write("ID de la cita: ");
            var citaIdStr = Console.ReadLine() ?? "";

            if (long.TryParse(citaIdStr, out long citaId))
            {
                try
                {
                    // Verificar que la cita existe antes de marcar como atendida
                    var cita = await _appointmentService.GetByIdAsync(citaId);
                    if (cita == null)
                    {
                        Console.WriteLine($"❌ Error: No se encontró una cita con ID {citaId}.");
                        Console.WriteLine("💡 Sugerencia: Verifique el ID o consulte la lista de citas.");
                        return;
                    }

                    if (cita.Status == AppointmentStatus.Atendida)
                    {
                        Console.WriteLine($"❌ Error: La cita con ID {citaId} ya está marcada como atendida.");
                        return;
                    }

                    if (cita.Status == AppointmentStatus.Cancelada)
                    {
                        Console.WriteLine($"❌ Error: No se puede marcar como atendida una cita cancelada.");
                        return;
                    }

                    await _appointmentService.MarkAsAttendedAsync(citaId);
                    Console.WriteLine("✅ Cita marcada como atendida exitosamente.");
                    Console.WriteLine($"📅 Fecha: {cita.AppointmentAt:dd/MM/yyyy HH:mm}");
            }
            catch (Exception ex)
            {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Detalle: {ex.InnerException.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Error: ID de cita debe ser un número válido.");
                Console.WriteLine($"💡 Usted ingresó: '{citaIdStr}'");
                Console.WriteLine("📝 Sugerencia: Ingrese solo números (ejemplo: 123)");
            }
        }

        private async Task ListarCitasPorPaciente()
        {
            Console.WriteLine("\n=== CITAS POR PACIENTE ===");
            Console.Write("Documento del paciente: ");
            var documento = Console.ReadLine() ?? "";

            try
            {
                var paciente = await _patientService.GetByDocumentAsync(documento);
                if (paciente == null)
                {
                    Console.WriteLine("Paciente no encontrado.");
                    return;
                }

                    var citas = await _appointmentService.GetByPatientIdAsync(paciente.Id);
                if (citas.Any())
                {
                    Console.WriteLine($"Citas de {paciente.FirstName} {paciente.LastName}:");
                    foreach (var cita in citas)
                    {
                        var medico = await _doctorService.GetByIdAsync(cita.DoctorId);
                        Console.WriteLine($"• ID: {cita.Id} - {cita.AppointmentAt:dd/MM/yyyy HH:mm} - Dr(a). {medico?.FullName} - Estado: {cita.Status}");
                }
            }
            else
            {
                    Console.WriteLine("No hay citas para este paciente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task ListarCitasPorMedico()
        {
            Console.WriteLine("\n=== CITAS POR MÉDICO ===");
            Console.Write("Documento del médico: ");
            var documento = Console.ReadLine() ?? "";

            try
            {
                var medico = await _doctorService.GetByDocumentAsync(documento);
                if (medico == null)
                {
                    Console.WriteLine("Médico no encontrado.");
                    return;
                }

                var citas = await _appointmentService.GetByDoctorIdAsync(medico.Id);
                if (citas.Any())
                {
                    Console.WriteLine($"Citas del Dr(a). {medico.FullName}:");
                    foreach (var cita in citas)
                    {
                        var paciente = await _patientService.GetByIdAsync(cita.PatientId);
                        Console.WriteLine($"• ID: {cita.Id} - {cita.AppointmentAt:dd/MM/yyyy HH:mm} - {paciente?.FirstName} {paciente?.LastName} - Estado: {cita.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("No hay citas para este médico.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task ListarEspecialidades()
        {
            Console.WriteLine("\n=== ESPECIALIDADES DISPONIBLES ===");
            try
            {
                var especialidades = await _specialtyService.GetAllAsync();
                if (especialidades.Any())
                {
                    foreach (var especialidad in especialidades)
                    {
                        Console.WriteLine($"• ID: {especialidad.Id} - {especialidad.Name}");
                }
            }
            else
            {
                    Console.WriteLine("No hay especialidades registradas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task ListarTodasLasCitas()
        {
            Console.WriteLine("\n=== TODAS LAS CITAS ===");
            try
            {
                var citas = await _appointmentService.GetAllAsync();
                if (citas.Any())
                {
                    foreach (var cita in citas)
                    {
                        var paciente = await _patientService.GetByIdAsync(cita.PatientId);
                        var medico = await _doctorService.GetByIdAsync(cita.DoctorId);
                        Console.WriteLine($"• ID: {cita.Id} - {cita.AppointmentAt:dd/MM/yyyy HH:mm}");
                        Console.WriteLine($"  Paciente: {paciente?.FirstName} {paciente?.LastName}");
                        Console.WriteLine($"  Médico: Dr(a). {medico?.FullName}");
                        Console.WriteLine($"  Estado: {cita.Status}");
                        if (!string.IsNullOrEmpty(cita.Reason))
                        {
                            Console.WriteLine($"  Motivo: {cita.Reason}");
                        }
                        Console.WriteLine();
                }
            }
            else
            {
                    Console.WriteLine("No hay citas registradas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalle: {ex.InnerException.Message}");
                }
            }
        }

        private async Task VerHistorialCorreos()
        {
            Console.WriteLine("\n=== HISTORIAL DE CORREOS ===");
            try
            {
                var correos = await _emailService.GetAllEmailLogsAsync();
                if (correos.Any())
                {
                    foreach (var correo in correos)
                    {
                        Console.WriteLine($"• {correo.SentAt:dd/MM/yyyy HH:mm} - Para: {correo.ToEmail} - Estado: {correo.Status}");
                        Console.WriteLine($"  Asunto: {correo.Subject}");
                        if (!string.IsNullOrEmpty(correo.ErrorMessage))
                        {
                            Console.WriteLine($"  Error: {correo.ErrorMessage}");
                        }
                }
            }
            else
            {
                    Console.WriteLine("No hay correos en el historial.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
