using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        //posteriormente agregare crear, eliminar y editar

        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                    string query = "select IdUsuario,Nombres,Apellidos,Correo,Clave,Reestablecer,Activo,FechaRegistro from USUARIO"; // Este es el query que se ejecutará para obtener los usuarios


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    //le digo que tipo de comando es, en este caso es tipo texto
                    cmd.CommandType = CommandType.Text;

                    // Abro la conexión
                    oconexion.Open();
                    // Aqui leemos todos los resultados de la ejecucion del Query
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Mientras este leyendo, almacena en la lista que hice
                        while (reader.Read())
                        {
                            lista.Add(new Usuario() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(reader["Reestablecer"]),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]).ToString("dd/MM/yyyy")
                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Usuario>();

            }
            return lista;
        }

        //metodo para ver si existe un correo en la base de datos
     


        // Método para registrar un usuario
        /// Este método recibe un objeto Usuario, y devuelve el ID generado y un mensaje de error si ocurre
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idGenerado = 0; // Variable para almacenar el ID generado
            Mensaje = string.Empty; // Inicializo el mensaje como vacío

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarUsuario", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarUsuario'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el usuario
                    comando.Parameters.AddWithValue("Nombres", obj.Nombres);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Apellidos", obj.Apellidos);// Apellidos del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Correo", obj.Correo);// Correo del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Clave", obj.Clave);// Clave del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Activo", obj.Activo);// Indico si el usuario está activo o no
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un string de hasta 500 caracteres
                    comando.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado

                    oconexion.Open(); // Abro la conexión a la base de datos
                    comando.ExecuteNonQuery(); // Ejecuto el comando

                    idGenerado = Convert.ToInt32(comando.Parameters["Resultado"].Value); // Obtengo el ID generado
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString(); // Obtengo el mensaje de error si lo hay
                }

            }
            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                idGenerado = 0; // Si hay un error, el ID generado será 0
                Mensaje = ex.Message;// Asigno el mensaje de error a la variable Mensaje
            }

            return idGenerado; // Retorno el ID generado (o 0 si hubo un error)
        }// fin del método Registrar


        // Método para editar un usuario existente
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la edición fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_EditarUsuario", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_EditarUsuario'
                    comando.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);// ID del usuario a editar
                    comando.Parameters.AddWithValue("Nombres", obj.Nombres);// Nombres del usuario, que se actualizarán en la base de datos
                    comando.Parameters.AddWithValue("Apellidos", obj.Apellidos);// Apellidos del usuario, que se actualizarán en la base de datos
                    comando.Parameters.AddWithValue("Correo", obj.Correo);// Correo del usuario, que se actualizará en la base de datos
                    comando.Parameters.AddWithValue("Activo", obj.Activo);// Indico si el usuario está activo o no
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un string de hasta 500 caracteres
                    comando.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado
                    oconexion.Open(); // Abro la conexión a la base de datos
                    comando.ExecuteNonQuery(); // Ejecuto el comando
                    int resultado = Convert.ToInt32(comando.Parameters["Resultado"].Value); // Obtengo el resultado de la operación
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString(); // Obtengo el mensaje de error si lo hay
                    respuesta = resultado == 1; // Si el resultado es mayor que 0, la edición fue exitosa, de lo contrario, fue fallida
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                respuesta = false; // Si hay un error, la respuesta será falsa
                Mensaje = ex.Message;// Asigno el mensaje de error a la variable Mensaje
            }
            return respuesta; // Retorno true si la edición fue exitosa, false si hubo un error
        }// fin del método



        //metodo para eliminar un usuario

        public bool Eliminar(int idUsuario, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la eliminación fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("delete top (1) from usuario where IdUsuario = @idUsuario", oconexion); // Creo un comando SQL para eliminar un usuario por su ID
                    comando.Parameters.AddWithValue("@idUsuario", idUsuario); // Agrego el parámetro del ID del usuario a eliminar
                    comando.CommandType = CommandType.Text; // Indico que es un comando de tipo texto
                    oconexion.Open(); // Abro la conexión a la base de datos

                    respuesta = comando.ExecuteNonQuery() > 0 ? true : false; // Ejecuto el comando y verifico si se eliminó al menos un registro, asignando true o false a la variable respuesta
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                respuesta = false; // Si hay un error, la respuesta será falsa
                Mensaje = ex.Message;// Asigno el mensaje de error a la variable Mensaje
            }
            return respuesta; // Retorno true si la eliminación fue exitosa, false si hubo un error

        }// fin del método Eliminar


        // Método para cambiar la clave de un usuario
        public bool CambiarClave(int idUsuario,string nuevaclave ,out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la eliminación fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("update usuario set clave = @nuevaclave, reestablecer = 0 where idusuario =@idUsuario", oconexion); // Creo un comando SQL para eliminar un usuario por su ID
                    comando.Parameters.AddWithValue("@idUsuario", idUsuario);
                    comando.Parameters.AddWithValue("@nuevaclave", nuevaclave);// agrego los parámetros necesarios para la consulta
                    comando.CommandType = CommandType.Text; // Indico que es un comando de tipo texto
                    oconexion.Open(); // Abro la conexión a la base de datos

                    respuesta = comando.ExecuteNonQuery() > 0 ? true : false; // Ejecuto el comando y verifico si se eliminó al menos un registro, asignando true o false a la variable respuesta
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                respuesta = false; // Si hay un error, la respuesta será falsa
                Mensaje = ex.Message;// Asigno el mensaje de error a la variable Mensaje
            }
            return respuesta; // Retorno true si la eliminación fue exitosa, false si hubo un error

        }

        public bool ReestablecerClave(int idUsuario, string clave, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la eliminación fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {// como va a reestablecer la clave, no se va a eliminar el usuario, solo se va a actualizar la clave y se va a poner reestablecer en 1
                    SqlCommand comando = new SqlCommand("update usuario set clave = @clave, reestablecer = 1 where idusuario = @idUsuario", oconexion); // Creo un comando SQL para eliminar un usuario por su ID
                    comando.Parameters.AddWithValue("@idUsuario", idUsuario);
                    comando.Parameters.AddWithValue("@clave", clave);// Agrego el parámetro del ID del usuario a eliminar
                    comando.CommandType = CommandType.Text; // Indico que es un comando de tipo texto
                    oconexion.Open(); // Abro la conexión a la base de datos

                    respuesta = comando.ExecuteNonQuery() > 0 ? true : false; // Ejecuto el comando y verifico si se eliminó al menos un registro, asignando true o false a la variable respuesta
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                respuesta = false; // Si hay un error, la respuesta será falsa
                Mensaje = ex.Message;// Asigno el mensaje de error a la variable Mensaje
            }
            return respuesta; // Retorno true si la eliminación fue exitosa, false si hubo un error

        }// fin del método Eliminar







    }// fin de la clase CD_Usuarios
}
