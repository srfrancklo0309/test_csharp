# 🏥 Hospital Management System - San Vicente Hospital

## 👨‍💻 Developer Information

**Name:** Emmanuel Pérez  
**Clan:** C#  
**Email:** emmanuelperezm123@gmail.com  
**ID Number:** 1033490277  
**Development Date:** 14/10/2025  

---

## 📋 Project Description

Hospital management system developed in C# with .NET 8.0, Entity Framework Core and MySQL. The system allows complete management of patients, doctors, specialties and medical appointments, including automatic sending of email confirmations using MailKit.

## ✨ Main Features

### 🧑‍🤝‍🧑 Patient Management
- ✅ **Complete registration** with robust validations
- ✅ **Edit existing information**
- ✅ **Unique document validation** (no duplicates)
- ✅ **Complete listing** with detailed information
- ✅ **Search by document** for quick queries
- ✅ **User-friendly validations** with descriptive error messages

### 👨‍⚕️ Doctor Management
- ✅ **Registration with associated specialties**
- ✅ **Edit medical information**
- ✅ **Unique document validation**
- ✅ **Unique name + specialty combination validation**
- ✅ **Listing filtered by specialty**
- ✅ **Active/inactive status management**
- ✅ **User-friendly validations** for better user experience

### 📅 Medical Appointment Management
- ✅ **Smart scheduling** with validations
- ✅ **Availability validation** for time slots (doctor and patient)
- ✅ **Appointment cancellation** with status validations
- ✅ **Mark as attended** with status controls
- ✅ **Multiple listings** (by patient, doctor, all appointments)
- ✅ **Automatic confirmation sending** via email
- ✅ **Date validations** (no appointments in the past)
- ✅ **User-friendly error messages** with suggestions

### 🏥 Specialty Management
- ✅ **Complete CRUD** of medical specialties
- ✅ **Unique name validation**
- ✅ **Prevention of deletion** if there are associated doctors
- ✅ **Complete listing** of available specialties

### 📧 Email System
- ✅ **Automatic sending** of appointment confirmations
- ✅ **Complete history** of sent emails
- ✅ **Sending status** (Sent, Not Sent, Error)
- ✅ **Professional HTML templates** for emails
- ✅ **MailKit integration** for robust sending
- ✅ **Flexible configuration** from appsettings.json

## 🛠️ Technologies Used

### **Framework and Language**
- **.NET 8.0** - Main framework
- **C# 12** - Programming language
- **Entity Framework Core 8.0** - Database ORM

### **Database**
- **MySQL 8.0+** - Database management system
- **Pomelo.EntityFrameworkCore.MySql** - MySQL provider for EF Core

### **Email**
- **MailKit 4.14.1** - Modern library for email sending
- **MimeKit** - MIME content handling

### **Patterns and Concepts**
- **Object-Oriented Programming (OOP)**
- **Repository Pattern** (implemented in services)
- **Dependency Injection** (simplified)
- **LINQ** for data queries
- **List<> and Dictionary<TKey, TValue>** for collections
- **Exception Handling** with Try-Catch
- **Robust validations** with user-friendly messages

## 📋 System Requirements

### **Required Software**
1. **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **MySQL 8.0+** - [Download here](https://dev.mysql.com/downloads/mysql/)
3. **Code editor** (Visual Studio 2022, Visual Studio Code, or JetBrains Rider)

### **Minimum System Requirements**
- **RAM:** 4GB minimum, 8GB recommended
- **Disk space:** 2GB free
- **Operating system:** Windows 10+, macOS 10.15+, or Linux (Ubuntu 18.04+)

## 🚀 Installation and Configuration Instructions

### **Step 1: Prepare the Environment**

#### **1.1 Install .NET 8.0 SDK**
```bash
# Verify installation
dotnet --version
# Should show: 8.0.x or higher
```

#### **1.2 Install MySQL**
- Download and install MySQL from [mysql.com](https://dev.mysql.com/downloads/mysql/)
- During installation, configure:
  - **Port:** 3306 (default)
  - **Root user:** Create secure password
  - **Service:** Configure to start automatically

#### **1.3 Verify MySQL**
```bash
# Connect to MySQL
mysql -u root -p
# Enter password when prompted
```

### **Step 2: Configure the Database**

#### **2.1 Create the Database**
```sql
-- Connect to MySQL as root
mysql -u root -p

-- Create the database
CREATE DATABASE h_sanvicente_epm CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Verify creation
SHOW DATABASES;
```

#### **2.2 Configure User (Optional but Recommended)**
```sql
-- Create specific user for the application
CREATE USER 'hospital_user'@'localhost' IDENTIFIED BY 'hospital_password_2024';

-- Grant full privileges on the database
GRANT ALL PRIVILEGES ON h_sanvicente_epm.* TO 'hospital_user'@'localhost';

-- Apply changes
FLUSH PRIVILEGES;

-- Verify user
SELECT User, Host FROM mysql.user WHERE User = 'hospital_user';
```

### **Step 3: Configure the Project**

#### **3.1 Clone/Download the Project**
```bash
# If you have Git
git clone <repository-url>
cd test-master

# Or download and extract the ZIP
```

#### **3.2 Configure Connection String**
Edit the `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=h_sanvicente_epm;Uid=hospital_user;Pwd=hospital_password_2024;"
  }
}
```

**Or if using root user:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=h_sanvicente_epm;Uid=root;Pwd=your_root_password;"
  }
}
```

#### **3.3 Configure Email (Optional but Recommended)**
Edit the `appsettings.json` file:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "FromEmail": "your-email@gmail.com",
    "FromName": "San Vicente Hospital",
    "EnableSsl": true,
    "UseAuthentication": true
  }
}
```

**To configure Gmail:**
1. Enable 2-step verification in your Google account
2. Generate an app password
3. Use that password in `SmtpPassword`

### **Step 4: Run the Project**

#### **4.1 Restore Dependencies**
```bash
# Navigate to project directory
cd test-master

# Restore NuGet packages
dotnet restore
```

#### **4.2 Build the Project**
```bash
# Build to verify no errors
dotnet build
```

#### **4.3 Run the Application**
```bash
# Run the application
dotnet run
```

## 📁 Project Structure

```
test-master/
├── 📁 Data/
│   └── HospitalDbContext.cs          # Entity Framework context
├── 📁 Models/
│   ├── Appointment.cs                # Medical Appointment entity
│   ├── Doctor.cs                     # Doctor entity
│   ├── EmailLog.cs                   # Email log entity
│   ├── Enums.cs                      # System enumerations
│   ├── Patient.cs                    # Patient entity
│   └── Specialty.cs                  # Specialty entity
├── 📁 Services/
│   ├── IAppointmentService.cs        # Appointment service interface
│   ├── AppointmentService.cs         # Appointment service implementation
│   ├── IDoctorService.cs             # Doctor service interface
│   ├── DoctorService.cs              # Doctor service implementation
│   ├── IEmailService.cs              # Email service interface
│   ├── EmailService.cs               # Email service implementation
│   ├── IPatientService.cs            # Patient service interface
│   ├── PatientService.cs             # Patient service implementation
│   ├── ISpecialtyService.cs          # Specialty service interface
│   └── SpecialtyService.cs           # Specialty service implementation
├── 📁 UI/
│   └── ConsoleUI.cs                  # Console user interface
├── 📄 appsettings.json               # Application configuration
├── 📄 EMAIL_SETUP.md                 # Email configuration documentation
├── 📄 Program.cs                     # Application entry point
├── 📄 README.md                      # This file
└── 📄 test.csproj                    # .NET project file
```

## 🎯 System Features

### **Main Menu**
```
=== HOSPITAL MANAGEMENT SYSTEM - SAN VICENTE HOSPITAL ===
1. Patient Management
2. Doctor Management  
3. Medical Appointment Management
4. Specialty Management
5. Email History
0. Exit
```

### **Patient Management**
- **Register Patient:** Document, name, last name, age, phone, email
- **Edit Patient:** Modify existing information
- **List Patients:** View all registered patients
- **Search Patient:** By document number

### **Doctor Management**
- **Register Doctor:** Document, full name, specialty, phone, email
- **Edit Doctor:** Modify existing information
- **List Doctors:** View all doctors (with specialty filter)
- **Search Doctor:** By document number

### **Medical Appointment Management**
- **Schedule Appointment:** Select patient, doctor, date and time
- **Cancel Appointment:** Cancel scheduled appointments
- **Mark as Attended:** Mark completed appointments
- **List Appointments by Patient:** View appointments for a specific patient
- **List Appointments by Doctor:** View appointments for a specific doctor
- **List All Appointments:** View all system appointments

### **Specialty Management**
- **List Specialties:** View all available specialties

### **Email History**
- **View Email Logs:** Complete history of sent emails

## ✅ Implemented Validations

### **Patient Validations**
- ✅ **Required document** and unique
- ✅ **Required name and last name**
- ✅ **Age between 0 and 120 years**
- ✅ **Valid email format** (if provided)
- ✅ **User-friendly error messages** with suggestions

### **Doctor Validations**
- ✅ **Required document** and unique
- ✅ **Required full name**
- ✅ **Valid specialty** (must exist)
- ✅ **Unique name + specialty combination**
- ✅ **Descriptive error messages**

### **Appointment Validations**
- ✅ **Date cannot be in the past**
- ✅ **Doctor available** at selected time
- ✅ **Patient available** at selected time
- ✅ **Valid date and time format**
- ✅ **Error messages with suggestions** to resolve conflicts

### **Specialty Validations**
- ✅ **Unique name** for specialties
- ✅ **Prevention of deletion** if there are associated doctors

## 🔧 Advanced Configuration

### **Remote Database Configuration**
If your database is on a remote server, update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVER_IP;Port=3306;Database=h_sanvicente_epm;Uid=username;Pwd=password;"
  }
}
```

### **Email Configuration for Other Providers**

#### **Outlook/Hotmail**
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp-mail.outlook.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@outlook.com",
    "SmtpPassword": "your-password",
    "FromEmail": "your-email@outlook.com",
    "FromName": "San Vicente Hospital",
    "EnableSsl": true,
    "UseAuthentication": true
  }
}
```

#### **Yahoo**
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.mail.yahoo.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@yahoo.com",
    "SmtpPassword": "your-app-password",
    "FromEmail": "your-email@yahoo.com",
    "FromName": "San Vicente Hospital",
    "EnableSsl": true,
    "UseAuthentication": true
  }
}
```

## 🚨 Troubleshooting

### **Database Connection Error**
```
❌ Error: An error occurred while saving the entity changes
```

**Solutions:**
1. **Verify MySQL is running:**
   ```bash
   # Windows
   net start mysql
   
   # Linux/macOS
   sudo systemctl start mysql
   ```

2. **Verify connection string** in `appsettings.json`

3. **Verify database exists:**
   ```sql
   SHOW DATABASES;
   ```

4. **Verify user permissions:**
   ```sql
   SHOW GRANTS FOR 'hospital_user'@'localhost';
   ```

### **Email Sending Error**
```
❌ Error: Authentication failed
```

**Solutions:**
1. **For Gmail:** Use app password, not normal password
2. **Verify 2-step verification** is enabled
3. **Verify SMTP configuration** in `appsettings.json`
4. **Check logs** in `email_logs` table

### **Build Error**
```
❌ Error: Package restore failed
```

**Solutions:**
1. **Restore packages:**
   ```bash
   dotnet restore
   ```

2. **Clean and rebuild:**
   ```bash
   dotnet clean
   dotnet build
   ```

3. **Verify .NET 8.0 installed:**
   ```bash
   dotnet --version
   ```

### **Dependency Error**
```
❌ Error: Could not load file or assembly
```

**Solutions:**
1. **Delete bin and obj folders:**
   ```bash
   rm -rf bin obj
   dotnet restore
   dotnet build
   ```

2. **Verify package versions** in `test.csproj`

## 📊 Recommended Workflow

### **First Run**
1. **Configure database** (create DB and user)
2. **Configure email** (optional but recommended)
3. **Run application** with `dotnet run`
4. **Verify connection** to database

### **Daily System Usage**
1. **Register specialties** (if needed)
2. **Register doctors** with their specialties
3. **Register patients** in the hospital
4. **Schedule appointments** between patients and doctors
5. **Manage appointments** (cancel, mark as attended)
6. **Review history** of sent emails

## 🎯 Acceptance Criteria Met

### **Technical Requirements**
- ✅ **Console application in C#**
- ✅ **Entity Framework Core** for data access
- ✅ **LINQ** for database queries
- ✅ **List<> and Dictionary<TKey, TValue>** for collections
- ✅ **MySQL** as database system
- ✅ **Object-Oriented Programming** implemented
- ✅ **Exception handling** with Try-Catch
- ✅ **Robust validations** in all entities

### **Required Features**
- ✅ **Complete CRUD** for all entities
- ✅ **Business validations** implemented
- ✅ **Robust error handling**
- ✅ **Intuitive user interface**
- ✅ **Automatic email sending**
- ✅ **Complete project documentation**

### **Code Quality**
- ✅ **Clean and well-structured code**
- ✅ **Separation of concerns** (Models, Services, UI)
- ✅ **Appropriate exception handling**
- ✅ **User-friendly validations**
- ✅ **Inline documentation** for critical code

## 📞 Support and Contact

**Developer:** Emmanuel Pérez  
**Email:** emmanuelperezm123@gmail.com  
**Last Update Date:** October 2025  

To report issues or request additional features, contact the developer.

## 📄 License

This project is part of an academic evaluation and meets all specified acceptance criteria. The code is for educational use and should not be used commercially without authorization.

---

**🏥 San Vicente Hospital - Hospital Management System**  
*Developed with ❤️ in C# and .NET 8.0*