using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
namespace CapaDatos
{
    public class CD_ubicacion
    {

        public List<Provincia> ObtenerProvincia()
        {
            List<Provincia> lista = new List<Provincia>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                    string query = "Select * from Provincia"; // Este es el query que se ejecutará para obtener los usuarios


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
                            lista.Add(new Provincia() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdProvincia = reader["IdProvincia"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                          
                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Provincia>();

            }
            return lista;
        }


        public List<Canton> ObtenerCanton(string idprovincia)
        {
            List<Canton> lista = new List<Canton>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                    string query = "SELECT * FROM Canton WHERE IdProvincia = @IdProvincia"; // Este es el query que se ejecutará para obtener los usuarios


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdProvincia", idprovincia);
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
                            lista.Add(new Canton() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdCanton = reader["IdCanton"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),

                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Canton>();

            }
            return lista;
        }


        public List<Distrito> ObtenerDistrito(string idprovincia, string idcan)
        {
            List<Distrito> lista = new List<Distrito>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    //hago un Query
                    string query = "Select * from Distrito where IdCan = @IdCanton and IdProvincia = @IdProvincia"; // Este es el query que se ejecutará para obtener los usuarios


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdCanton", idcan);
                    cmd.Parameters.AddWithValue("@IdProvincia", idprovincia);
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
                            lista.Add(new Distrito() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdDistrito = reader["IdDistrito"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),

                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Distrito>();

            }
            return lista;
        }



        // para las facturas
        public Distrito ObtenerDatosUbicacionPorDistrito(string idDistrito)
        {
            Distrito oDistrito = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn))
                {
                    string query = @"SELECT d.IdDistrito, d.Descripcion AS DistritoDescripcion,
                                    c.IdCanton, c.Descripcion AS CantonDescripcion,
                                    p.IdProvincia, p.Descripcion AS ProvinciaDescripcion
                             FROM Distrito d
                             INNER JOIN Canton c ON d.IdCan = c.IdCanton
                             INNER JOIN Provincia p ON c.IdProvincia = p.IdProvincia
                             WHERE d.IdDistrito = @IdDistrito";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@IdDistrito", idDistrito);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            oDistrito = new Distrito()
                            {
                                IdDistrito = reader["IdDistrito"].ToString(),
                                Descripcion = reader["DistritoDescripcion"].ToString(),
                                ProvinciaDescripcion = reader["ProvinciaDescripcion"].ToString(),
                                CantonDescripcion = reader["CantonDescripcion"].ToString()
                            };
                        }
                    }
                }
            }
            catch
            {
                oDistrito = null;
            }

            return oDistrito;
        }




    }
}
