using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace CapaDatos
{
    public class CD_Marca

    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                    string query = "select IdMarca, Descripcion, Activo from MARCA"; // Este es el query que se ejecutará para obtener los usuarios


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
                            lista.Add(new Marca() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"]),

                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Marca>();

            }
            return lista;
        }//



        public int Registrar(Marca obj, out string Mensaje)
        {
            int idGenerado = 0; // Variable para almacenar el ID generado
            Mensaje = string.Empty; // Inicializo el mensaje como vacío

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarMarca", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarUsuario'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el usuario
                    comando.Parameters.AddWithValue("Descripcion", obj.Descripcion);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Activo", obj.Activo);// Apellidos del usuario, que se almacenará en la base de dato
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



        public bool Editar(Marca obj, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la edición fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_EditarMarca", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_EditarUsuario'
                    comando.Parameters.AddWithValue("IdMarca", obj.IdMarca);// ID de la categoria
                    comando.Parameters.AddWithValue("Descripcion", obj.Descripcion);// Descripción de la categoría, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Activo", obj.Activo);// Indico si el usuario está activo o no
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un string de hasta 500 caracteres
                    comando.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado
                    oconexion.Open(); // Abro la conexión a la base de datos
                    comando.ExecuteNonQuery(); // Ejecuto el comando
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString(); // Obtengo el mensaje de error si lo hay
                   int resultado = Convert.ToInt32(comando.Parameters["Resultado"].Value); // Obtengo el resultado de la operación
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



        public bool Eliminar(int id, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la edición fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_EliminarMarca", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_EditarUsuario'
                    comando.Parameters.AddWithValue("IdMarca", id);// ID de la marca a eliminar
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un string de hasta 500 caracteres
                    comando.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado
                    oconexion.Open(); // Abro la conexión a la base de datos
                    comando.ExecuteNonQuery(); // Ejecuto el comando
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString(); // Obtengo el mensaje de error si lo hay
                    int resultadoSP = Convert.ToInt32(comando.Parameters["Resultado"].Value);
                    respuesta = resultadoSP == 1; // Si todo sale bien, indico que la edición fue exitosa
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




        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                   StringBuilder stringBuilder = new StringBuilder();

                    
                    //
                    //
                    //

                    stringBuilder.AppendLine("select distinct m.IdMarca, m.Descripcion from PRODUCTO p");
                    stringBuilder.AppendLine("inner join Categoria c on c.idCategoria = p.IdCategoria");
                    stringBuilder.AppendLine("inner join MARCA m on m.IdMarca = p.IdMarca and m.Activo = 1");
                    stringBuilder.AppendLine("where c.idCategoria = iif(@idcategoria = 0, c.idCategoria, @idcategoria)");

                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand(stringBuilder.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
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
                            lista.Add(new Marca() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                Descripcion = reader["Descripcion"].ToString(),
                            

                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Marca>();

            }
            return lista;
        }//
    }
}//
