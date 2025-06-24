using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Carrito
    {

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_ExisteCarrito", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarUsuario'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el usuario
                    comando.Parameters.AddWithValue("IdCliente", idcliente);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("IdProducto", idproducto);// Apellidos del usuario, que se almacenará en la base de datos
                
                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                   
                    comando.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado

                    oconexion.Open(); // Abro la conexión a la base de datos
                    comando.ExecuteNonQuery(); // Ejecuto el comando

                   resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value); // Obtengo el ID generado
                  
                }

            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado; // Retorno el ID generado (o 0 si hubo un error)
        }// fin del método Existe carrito











        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string mensaje)
        {
            bool resultado = true;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_OperacionCarrito", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarUsuario'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el usuario
                    comando.Parameters.AddWithValue("IdCliente", idcliente);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("IdProducto", idproducto);// Apellidos del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Sumar", sumar);
                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un string de hasta 500 caracteres
                    comando.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado

                    oconexion.Open(); // Abro la conexión a la base de datos
                    comando.ExecuteNonQuery(); // Ejecuto el comando

                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value); // Obtengo el ID generado
                    mensaje = comando.Parameters["Mensaje"].Value.ToString(); // Obtengo el mensaje de error si lo hay
                }

              

            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado; // Retorno el ID generado (o 0 si hubo un error)
        }// fin del metodo


        //

        public int CantidadEnCarrito(int idcliente)
        {
            int respuesta = 0; // Variable para indicar si la eliminación fue exitosa
  
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("select count(*) from carrito where idcliente = @idcliente", oconexion); // Creo un comando SQL para eliminar un usuario por su ID
                    comando.Parameters.AddWithValue("@idcliente", idcliente); // Agrego el parámetro del ID del usuario a eliminar
                    comando.CommandType = CommandType.Text; // Indico que es un comando de tipo texto
                    oconexion.Open(); // Abro la conexión a la base de datos

                    respuesta = Convert.ToInt32(comando.ExecuteScalar());// Ejecuto el comando y verifico si se eliminó al menos un registro, asignando true o false a la variable respuesta
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                respuesta = 0; // Si hay un error, la respuesta será falsa
               
            }
            return respuesta; // Retorno true si la eliminación fue exitosa, false si hubo un error

        }// fin del cantidad



        public List<Carrito> ListarProducto(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {



                    string query = "select * from fn_obtenerCarritoCliente(@idcliente)";


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
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
                            lista.Add(new Carrito() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {



                                oProducto = new Producto()
                                {
                                    IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    oMarca = new Marca()
                                    {
                                        Descripcion = reader["Marca"].ToString()
                                    },

                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-CR")),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString()
                                },


                                Cantidad = Convert.ToInt32(reader["Cantidad"])
                            }); 

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Carrito>();

            }
            return lista;
        }// fin del metodo Listar producto en carrito


        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            /* Como he usado muchos metodos hay comentarios parecidos
             sin embargo, est metodo lo que hará es eliminar cosas del carrito 
            al final mucha logica es parecida, pero cambian los sp*/

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) 
                {
                    SqlCommand comando = new SqlCommand("sp_EliminarCarrito", oconexion);
                    comando.Parameters.AddWithValue("IdCliente", idcliente);
                    comando.Parameters.AddWithValue("IdProducto", idproducto);

                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    comando.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); 
                    comando.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value); 
                }

            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado; 
        }// fin del método Existe carrito





    }
}
