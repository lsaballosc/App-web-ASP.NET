using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idGenerado = 0; // Variable para almacenar el ID generado
            Mensaje = string.Empty; // Inicializo el mensaje como vacío

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarCliente", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarCliente'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el Cliente
                    comando.Parameters.AddWithValue("Nombres", obj.Nombres);// Nombres del Cliente, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Apellidos", obj.Apellidos);// Apellidos del Cliente, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Correo", obj.Correo);// Correo del Cliente, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Clave", obj.Clave);// Clave del Cliente, que se almacenará en la base de dato
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


        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                    string query = "select IdCliente,Nombres,Apellidos,Correo,Clave,Reestablecer from Cliente"; // Este es el query que se ejecutará para obtener los Clientes


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
                            lista.Add(new Cliente() // Aquí creo un nuevo objeto Cliente y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Cliente
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(reader["Reestablecer"]),

                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Cliente>();

            }
            return lista;
        }// fin del método Listar

        public bool CambiarClave(int idCliente, string nuevaclave, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la eliminación fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("update Cliente set clave = @nuevaclave, reestablecer = 0 where idCliente =@idCliente", oconexion); // Creo un comando SQL para eliminar un Cliente por su ID
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
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

        }/// fin del método CambiarClave


        public bool ReestablecerClave(int idCliente, string clave, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la eliminación fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {// como va a reestablecer la clave, no se va a eliminar el Cliente, solo se va a actualizar la clave y se va a poner reestablecer en 1
                    SqlCommand comando = new SqlCommand("update Cliente set clave = @clave, reestablecer = 1 where idCliente = @idCliente", oconexion); // Creo un comando SQL para eliminar un Cliente por su ID
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    comando.Parameters.AddWithValue("@clave", clave);// Agrego el parámetro del ID del Cliente a eliminar
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

        }// fin del método ReestablecerClave
    }
}
