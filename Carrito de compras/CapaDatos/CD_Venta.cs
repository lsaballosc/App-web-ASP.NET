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
    public class CD_Venta
    {

        // metodo que permite registrar ka venta

        public bool Registrar(Venta obj,DataTable DetalleVenta ,out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la edición fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("usp_RegistrarVenta", oconexion); 
                    comando.Parameters.AddWithValue("IdCliente", obj.IdCliente);
                    comando.Parameters.AddWithValue("TotalProducto", obj.TotalProducto);
                    comando.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    comando.Parameters.AddWithValue("Contacto", obj.Contacto);
                    comando.Parameters.AddWithValue("IdDistrito", obj.IdDistrito);
                    comando.Parameters.AddWithValue("Telefono", obj.Telefono);
                    comando.Parameters.AddWithValue("Direccion", obj.Direccion);
                    comando.Parameters.AddWithValue("IdTransaccion", obj.IdTransaccion);
               
                    comando.Parameters.AddWithValue("DetalleVenta", DetalleVenta);




                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
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





        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {



                    string query = "select * from fn_ListarCompra(@idcliente)";


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
                            lista.Add(new DetalleVenta() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {



                                oProducto = new Producto()
                                {
                                    
                                    Nombre = reader["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-CR")),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString()
                                },


                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"], new CultureInfo("es-CR")),
                                IdTransaccion = reader["IdTransaccion"].ToString(),
                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<DetalleVenta>();

            }
            return lista;
        }// fin del metodo Listar para ver historico de compras

    }
}
