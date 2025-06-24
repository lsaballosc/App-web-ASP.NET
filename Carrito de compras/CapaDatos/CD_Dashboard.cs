using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
namespace CapaDatos
{
    public class CD_Dashboard
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                   


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("@fechaInicio", fechainicio); // Agrego el parámetro de fecha de inicio
                    cmd.Parameters.AddWithValue("@fechaFin", fechafin); // Agrego el parámetro de fecha de fin
                    cmd.Parameters.AddWithValue("@idtransaccion", idtransaccion); // Agrego el pará
                    //le digo que tipo de comando es, en este caso es tipo texto
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Abro la conexión
                    oconexion.Open();
                    // Aqui leemos todos los resultados de la ejecucion del Query
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Mientras este leyendo, almacena en la lista que hice
                        while (reader.Read())
                        {
                            lista.Add(new Reporte() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                FechaVenta = reader["FechaVenta"].ToString(),
                                Cliente = reader["Cliente"].ToString(),
                                Producto = reader["Producto"].ToString(),
                                Precio = Convert.ToDecimal(   reader["Precio"],new CultureInfo("es-CR")),
                                Cantidad = Convert.ToInt32( reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"], new CultureInfo("es-CR")),
                                IdTransaccion = reader["IdTransaccion"].ToString()
                               
                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Reporte>();

            }
            return lista;
        }



        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                  


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconexion);
                    //le digo que tipo de comando es, en este caso es tipo texto
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Abro la conexión
                    oconexion.Open();
                    // Aqui leemos todos los resultados de la ejecucion del Query
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Mientras este leyendo, almacena en la lista que hice
                        while (reader.Read())
                        {
                            objeto = new Dashboard()
                            {
                                totalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                totalVenta = Convert.ToInt32(reader["TotalVenta"]),
                                totalProducto = Convert.ToInt32(reader["TotalProducto"])
                               
                            };

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                objeto = new Dashboard();

            }
            return objeto;
        }

    }
}
