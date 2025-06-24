using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {

        private CD_Cliente objCdCliente = new CD_Cliente();

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(obj.Nombres) ||
                string.IsNullOrWhiteSpace(obj.Apellidos) ||
                string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "Los campos Nombres, Apellidos, Correo y Clave son obligatorios.";
                return 0;
            }

            // 1. Generar la clave temporal
            string claveTemporal = obj.Clave;

            // 2. Encriptar la clave temporal y asignarla al objeto
            obj.Clave = CN_Recursos.EncriptarSHA256(claveTemporal);

            // 3. Intentar registrar el Cliente en la base de datos
            int idGenerado = objCdCliente.Registrar(obj, out Mensaje);

            // 4. Solo si se registró con éxito (id > 0), enviar el correo
            if (string.IsNullOrEmpty(Mensaje))
            {
                if (idGenerado > 0)
                {
                    obj.Clave = CN_Recursos.EncriptarSHA256(claveTemporal); // Asegurar que la clave esté encriptada antes de enviar el correo
                   // return objCdCliente.Registrar(obj, out Mensaje); // Registrar el Cliente y obtener el ID generado
                }
            }
            else
            {
                return 0; // Si hubo un error al registrar, retorno 0
            }


                // Retorna 0 si no se registró, o el ID generado si todo está bien
                return idGenerado;
        }// Registrar

        public List<Cliente> Listar()
        {
            //llamo al metodo de la capa de datos que me devuelve una lista de Clientes
            return objCdCliente.Listar();
        }




        public bool CambiarClave(int idCliente, string nuevaclave, out string Mensaje)
        {
            return objCdCliente.CambiarClave(idCliente, nuevaclave, out Mensaje); // Llamo al método de la capa de datos para eliminar el Cliente y obtengo el mensaje de error si ocurre
        }




        public bool ReestablecerClave(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            // 1. Generar la clave temporal
            string nuevaClave = CN_Recursos.GenerarClave();

            bool resultado = objCdCliente.ReestablecerClave(idCliente, CN_Recursos.EncriptarSHA256(nuevaClave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                // string cuerpo = $"<h1>Bienvenido a nuestro sistema </h1><p>Su cuenta ha sido creada exitosamente {obj.Nombres} </p><p>Su clave de acceso es: <strong>{claveTemporal}</strong></p>";
                string cuerpo = $@"
                    <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                        <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); overflow: hidden;'>
                            <div style='background-color: #2E86C1; color: white; padding: 20px; text-align: center;'>
                                <h1 style='margin: 0;'>¡Su cuenta fue reestablecida correctamente!</h1>
                            </div>
                            <div style='padding: 30px;'>
                              
                                <p style='font-size: 16px;'>Tu clave de acceso es:</p>
                                <p style='font-size: 18px; font-weight: bold; color: #2E86C1;'>{nuevaClave}</p>
                                <hr style='margin: 30px 0;'>
                                <p style='font-size: 14px; color: #777;'>Por seguridad, deberás cambiar la clave una vez que ingreses al sistema.</p>
                            </div>
                            <div style='background-color: #f0f0f0; padding: 15px; text-align: center; font-size: 12px; color: #888;'>
                                © 2025 JzelaCrochet. Todos los derechos reservados.
                            </div>
                        </div>
                    </div>";



                bool enviado = CN_Recursos.EnviarCorreo(correo, asunto, cuerpo);

                if (enviado)
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo de reestablecimiento de clave. Por favor, intente nuevamente.";
                    return false;
                }

            }
            else
            {
                Mensaje = "No se pudo reestablecer la clave. Por favor, intente nuevamente.";
                return false;
            }





        }
    }
}
