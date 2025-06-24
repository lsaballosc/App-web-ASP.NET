using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class CN_Usuarios
    {
        // creo objeto de la capa de datos para poder acceder a los metodos de la capa de datos
        private CD_Usuarios  objCdUsuario = new CD_Usuarios();



        // metodo para listar los usuarios
        public List<Usuario> Listar()
        {
            //llamo al metodo de la capa de datos que me devuelve una lista de usuarios
            return objCdUsuario.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
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
            string claveTemporal = CN_Recursos.GenerarClave();

            // 2. Encriptar la clave temporal y asignarla al objeto
            obj.Clave = CN_Recursos.EncriptarSHA256(claveTemporal);

            // 3. Intentar registrar el usuario en la base de datos
            int idGenerado = objCdUsuario.Registrar(obj, out Mensaje);

            // 4. Solo si se registró con éxito (id > 0), enviar el correo
            if (string.IsNullOrEmpty(Mensaje))
            {
                if (idGenerado > 0)
                {
                    string asunto = "Creación de Cuenta";
                    // string cuerpo = $"<h1>Bienvenido a nuestro sistema </h1><p>Su cuenta ha sido creada exitosamente {obj.Nombres} </p><p>Su clave de acceso es: <strong>{claveTemporal}</strong></p>";
                    string cuerpo = $@"
                    <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                        <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1); overflow: hidden;'>
                            <div style='background-color: #2E86C1; color: white; padding: 20px; text-align: center;'>
                                <h1 style='margin: 0;'>¡Bienvenido, {obj.Nombres} {obj.Apellidos}!</h1>
                            </div>
                            <div style='padding: 30px;'>
                                <p style='font-size: 16px;'>Nos complace darte la bienvenida a nuestro sistema.</p>
                                <p style='font-size: 16px;'>Tu cuenta ha sido creada exitosamente.</p>
                                <p style='font-size: 16px;'>Tu clave de acceso es:</p>
                                <p style='font-size: 18px; font-weight: bold; color: #2E86C1;'>{claveTemporal}</p>
                                <hr style='margin: 30px 0;'>
                                <p style='font-size: 14px; color: #777;'>Por seguridad, deberás cambiar la clave una vez que ingreses al sistema.</p>
                            </div>
                            <div style='background-color: #f0f0f0; padding: 15px; text-align: center; font-size: 12px; color: #888;'>
                                © 2025 JzelaCrochet. Todos los derechos reservados.
                            </div>
                        </div>
                    </div>";



                    bool enviado = CN_Recursos.EnviarCorreo(obj.Correo, asunto, cuerpo);

                    if (!enviado)
                    {
                        Mensaje = "No se puede ingresar el usuario porque el correo ya existe .";
                    }
                }
            }
               

            // Retorna 0 si no se registró, o el ID generado si todo está bien
            return idGenerado;
        }


    


        // Metodo para actualizar un usuario
        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "Los campos Nombres, Apellidos, Correo y Clave son obligatorios.";
                return false;
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los campos Nombres, Apellidos, Correo y Clave son obligatorios.";
                return false;
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "Los campos Nombres, Apellidos, Correo y Clave son obligatorios.";
                return false;
            }
            //si el mensaje esta vacio, significa que no hay errores en los campos obligatorios
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCdUsuario.Editar(obj, out Mensaje); // Llamo al método de la capa de datos para editar el usuario y obtengo el mensaje de error si ocurre
            }
            else
            {
                return false; // Retorno false si hay un mensaje de error, indicando que no se pudo editar
            }
        }



        // Metodo para eliminar un usuario
        public bool Eliminar(int idUsuario, out string Mensaje)
        {
            return objCdUsuario.Eliminar(idUsuario, out Mensaje); // Llamo al método de la capa de datos para eliminar el usuario y obtengo el mensaje de error si ocurre
        }

        public bool CambiarClave(int idUsuario, string nuevaclave, out string Mensaje)
        {
            return objCdUsuario.CambiarClave(idUsuario,nuevaclave ,out Mensaje); // Llamo al método de la capa de datos para eliminar el usuario y obtengo el mensaje de error si ocurre
        }


        //metodo para reestablecer la clave
        public bool ReestablecerClave(int idusuario,string correo ,out string Mensaje)
        {
            Mensaje = string.Empty;

            // 1. Generar la clave temporal
            string nuevaClave = CN_Recursos.GenerarClave();

            bool resultado = objCdUsuario.ReestablecerClave(idusuario,CN_Recursos.EncriptarSHA256(nuevaClave) ,out Mensaje);

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

               if(enviado)
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
