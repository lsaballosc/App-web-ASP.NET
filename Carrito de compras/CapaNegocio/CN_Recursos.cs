using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO; // Importar para manejar archivos y directorios
using System.Linq;
using System.Net; // Importar para usar NetworkCredential y SmtpClient
using System.Net.Mail; // Importar para enviar correos electrónicos
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace CapaNegocio
{
    public class CN_Recursos
    {

        //metodo para generar clave aleatoria
        public static string GenerarClave()
        {
            //un GUID es un identificador único global que se puede usar para generar una clave aleatoria
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6); // Generar un GUID y tomar los primeros 8 caracteres
            return clave; // Retornar la clave generada
        }





        //Metodo para encriptar la clave con SHA256
        public static string EncriptarSHA256(string texto)
        {
            StringBuilder sb = new StringBuilder(); // Usar StringBuilder para construir el hash en formato hexadecimal
            // usar la referencia System.Security.Cryptography para usar SHA256
            using (SHA256 hash = SHA256Managed.Create())// Crear una instancia de SHA256Managed para calcular el hash
            {
                Encoding enc = Encoding.UTF8; // Usar UTF8 para codificar el texto a bytes
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));// Calcular el hash del texto convertido a bytes

                foreach (byte b in result) // Recorrer cada byte del resultado del hash
                {
                    sb.Append(b.ToString("x2")); // Convertir cada byte a hexadecimal y agregarlo al StringBuilder
                }
            }

            return sb.ToString(); // Retornar el hash en formato hexadecimal
        }


        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {

            bool resultado = false; // Variable para almacenar el resultado del envío de correo

            try
            {
                MailMessage mail = new MailMessage(); // Crear una instancia de MailMessage para configurar el correo
                mail.To.Add(correo); // Agregar el destinatario al correo
                mail.From = new MailAddress("jzelacrochet@gmail.com");
                mail.Subject = asunto; // Establecer el asunto del correo
                mail.Body = mensaje; // Establecer el cuerpo del correo
                mail.IsBodyHtml = true; // Indicar que el cuerpo del correo es HTML

                var servidorSmtp = new SmtpClient("smtp.gmail.com", 587)
                //si da error probar poner la contraseña pegada sin espacios ni comillas
                {
                    Credentials = new NetworkCredential("jzelacrochet@gmail.com", "mshigfbajjrvtpwr"), // configurar las credenciales del servidor SMTP de Gmail
                    // Configurar las credenciales del servidor SMTP de Gmail
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true // Habilitar SSL para el envío seguro de correos

                };// Configurar el servidor SMTP de Gmail, se encarga // de enviar el correo

                servidorSmtp.Send(mail); // Enviar el correo utilizando el servidor SMTP configurado
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false; // Si ocurre una excepción, establecer el resultado como false
            }

            return resultado; // Retornar el resultado del envío de correo
        }




        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty; // Inicializar la variable para almacenar el texto en Base64
            conversion = true; // Inicializar la variable de conversión como verdadera


            try
            {
                byte[] bytes = File.ReadAllBytes(ruta); // Leer todos los bytes del archivo en la ruta especificada
                textoBase64 = Convert.ToBase64String(bytes); // Convertir los bytes a una cadena en Base64
            }
            catch (Exception ex)
            {
                conversion = false; // Si ocurre una excepción, establecer la conversión como falsa
            }
            return textoBase64; // Retornar la cadena en Base64
        }




        public static void EnviarFacturaCorreo(string correoCliente, string nombreCliente, Venta venta)
        {
            if (venta.oDetalleVenta == null || venta.oDetalleVenta.Count == 0)
                return;

            string correoTienda = "jzelacrochet@gmail.com";
            string asunto = "🧾 Factura de compra - JzelaCrochet";

            string cuerpo = $@"
<div style='max-width: 700px; margin: auto; font-family: Arial, sans-serif; border: 1px solid #ccc;'>

    <!-- Encabezado -->
    <div style='background-color: #1A5276; color: white; padding: 20px; text-align: center;'>
        <h1 style='margin: 0;'>FACTURA</h1>
        <p style='margin: 5px 0;'>FECHA: {DateTime.Now:dd/MM/yyyy}</p>
        <p style='margin: 5px 0;'>TRANSACCIÓN #: {venta.IdTransaccion}</p>
    </div>

    <!-- Información cliente y empresa -->
    <div style='display: flex; justify-content: space-between; padding: 20px; font-size: 14px;'>
        <div style='width: 48%;'>
            <strong>CLIENTE:</strong><br>
            <strong>Usuario logueado:</strong> {nombreCliente}<br>
            <strong>Nombre:</strong> {venta.Contacto}<br>
            <strong>Teléfono:</strong> {venta.Telefono}<br>
            <strong>Ubicación:</strong> {venta.ProvinciaDescripcion}, {venta.CantonDescripcion}, {venta.DistritoDescripcion}<br>
            <strong>Dirección exacta:</strong> {venta.Direccion}
        </div>
        <!-- Empresa alineada a la derecha -->
    <div style='width: 48%; text-align: right;'>
        <strong style='color: #6A1B9A;'>EMPRESA:</strong><br>
        <span style='font-weight: bold;'>JzelaCrochet</span><br>
        <a href='mailto:jzelacrochet@gmail.com' style='color: #1A237E;'>jzelacrochet@gmail.com</a><br>
        <span>Liberia, Guanacaste, Costa Rica</span><br>
        <img src='https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Check_green_icon.svg/1200px-Check_green_icon.svg.png' alt='Logo' width='50' style='margin-top: 10px;' />
    </div>
    </div>

    <!-- Detalle de productos -->
    <table style='width: 100%; border-collapse: collapse; font-size: 14px;'>
        <thead>
            <tr style='background-color: #1A5276; color: white;'>
                <th style='padding: 10px; border: 1px solid #ddd;'>DESCRIPCIÓN</th>
                <th style='padding: 10px; border: 1px solid #ddd;'>CANTIDAD</th>
                <th style='padding: 10px; border: 1px solid #ddd;'>PRECIO UNITARIO (₡)</th>
                <th style='padding: 10px; border: 1px solid #ddd;'>IMPORTE (₡)</th>
            </tr>
        </thead>
        <tbody>";

            decimal total = 0;

            foreach (var detalle in venta.oDetalleVenta)
            {
                string producto = detalle.oProducto.Nombre;
                int cantidad = detalle.Cantidad;
                decimal subtotal = detalle.Total;
                decimal precioUnitario = cantidad > 0 ? subtotal / cantidad : 0;

                cuerpo += $@"
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'>{producto}</td>
                <td style='padding: 10px; border: 1px solid #ddd; text-align: center;'>{cantidad}</td>
                <td style='padding: 10px; border: 1px solid #ddd; text-align: right;'>{precioUnitario:N2}</td>
                <td style='padding: 10px; border: 1px solid #ddd; text-align: right;'>{subtotal:N2}</td>
            </tr>";

                total += subtotal;
            }

            cuerpo += $@"
            <tr>
                <td colspan='3' style='padding: 10px; border: 1px solid #ddd; text-align: right; font-weight: bold;'>TOTAL</td>
                <td style='padding: 10px; border: 1px solid #ddd; text-align: right; font-weight: bold;'>₡{total:N2}</td>
            </tr>
        </tbody>
    </table>

    <!-- Footer -->
    <div style='padding: 20px; font-size: 12px; color: #555; text-align: center;'>
        <p>Gracias por tu compra 💙</p>
        <p><strong>⚠️ Te recomendamos guardar este correo como comprobante en caso de consultas o reclamos.</strong></p>
        <p>© 2025 JzelaCrochet. Todos los derechos reservados.</p>
    </div>
</div>";
            

            if (!string.IsNullOrWhiteSpace(correoCliente))
                EnviarCorreo(correoCliente, asunto, cuerpo);

            EnviarCorreo(correoTienda, "🔔 Nueva compra efectuada", cuerpo);
        }

    



    }
}