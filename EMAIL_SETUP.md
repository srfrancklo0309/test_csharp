# 📧 Configuración de Email con MailKit

Este documento explica cómo configurar el envío de correos electrónicos en el sistema de gestión hospitalaria usando MailKit.

## 🔧 Configuración Requerida

### 1. Gmail (Recomendado)

Para usar Gmail como proveedor de email, necesitas:

1. **Habilitar la verificación en 2 pasos** en tu cuenta de Google
2. **Generar una contraseña de aplicación**:
   - Ve a [myaccount.google.com](https://myaccount.google.com)
   - Seguridad → Verificación en 2 pasos
   - Contraseñas de aplicaciones
   - Genera una nueva contraseña para "Mail"

### 2. Actualizar appsettings.json

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "tu-email@gmail.com",
    "SmtpPassword": "tu-app-password-aqui",
    "FromEmail": "tu-email@gmail.com",
    "FromName": "Hospital San Vicente",
    "EnableSsl": true,
    "UseAuthentication": true
  }
}
```

### 3. Otros Proveedores

#### Outlook/Hotmail
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp-mail.outlook.com",
    "SmtpPort": 587,
    "SmtpUsername": "tu-email@outlook.com",
    "SmtpPassword": "tu-password",
    "FromEmail": "tu-email@outlook.com",
    "FromName": "Hospital San Vicente",
    "EnableSsl": true,
    "UseAuthentication": true
  }
}
```

#### Yahoo
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.mail.yahoo.com",
    "SmtpPort": 587,
    "SmtpUsername": "tu-email@yahoo.com",
    "SmtpPassword": "tu-app-password",
    "FromEmail": "tu-email@yahoo.com",
    "FromName": "Hospital San Vicente",
    "EnableSsl": true,
    "UseAuthentication": true
  }
}
```

## 🚀 Características de MailKit

### ✅ Ventajas sobre System.Net.Mail

- **Mejor rendimiento**: Más rápido y eficiente
- **Más opciones de seguridad**: Soporte completo para TLS/SSL
- **Mejor manejo de errores**: Mensajes de error más descriptivos
- **Soporte para MIME**: Mejor manejo de contenido HTML y adjuntos
- **Activamente mantenido**: Actualizaciones regulares y soporte

### 🔒 Seguridad

- **TLS/SSL**: Conexiones encriptadas por defecto
- **Autenticación segura**: Soporte para OAuth2 y contraseñas de aplicación
- **Validación de certificados**: Verificación automática de certificados SSL

## 📝 Uso en el Sistema

### Envío Automático de Confirmaciones

Cuando se agenda una cita, el sistema automáticamente:

1. **Genera un email HTML** con los detalles de la cita
2. **Envía el email** al paciente
3. **Registra el envío** en la base de datos
4. **Maneja errores** graciosamente

### Plantilla de Email

El sistema incluye una plantilla HTML profesional con:

- **Diseño responsive** para móviles y escritorio
- **Información completa** de la cita
- **Instrucciones importantes** para el paciente
- **Branding del hospital**

## 🛠️ Solución de Problemas

### Error: "Authentication failed"

**Causa**: Credenciales incorrectas o contraseña de aplicación no configurada

**Solución**:
1. Verifica que uses una contraseña de aplicación (no tu contraseña normal)
2. Asegúrate de que la verificación en 2 pasos esté habilitada
3. Verifica que el email y contraseña sean correctos

### Error: "Connection timeout"

**Causa**: Problemas de red o configuración de puerto

**Solución**:
1. Verifica tu conexión a internet
2. Confirma que el puerto 587 esté abierto
3. Prueba con un puerto alternativo (465 para SSL)

### Error: "SSL/TLS handshake failed"

**Causa**: Problemas con certificados SSL

**Solución**:
1. Verifica que `EnableSsl` esté en `true`
2. Actualiza MailKit a la versión más reciente
3. Verifica la fecha y hora del sistema

## 📊 Monitoreo

El sistema registra todos los envíos de email en la tabla `email_logs` con:

- **Estado del envío**: Enviado, Error, No enviado
- **Timestamp**: Fecha y hora del envío
- **Mensaje de error**: Si el envío falla
- **Detalles de la cita**: Relación con la cita médica

## 🔄 Próximas Mejoras

- [ ] Soporte para adjuntos (recetas, reportes)
- [ ] Plantillas personalizables
- [ ] Envío masivo de notificaciones
- [ ] Integración con servicios de email marketing
- [ ] Notificaciones push para móviles
