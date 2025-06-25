using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CapaDatos
{
    public class CD_Productos
    {

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();


            try
            {
                //La cadena de conexión se obtiene desde la clase Conection, en la propiedad cn que obtiene la conexión
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection

                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(" select p.IdProducto,p.Nombre,p.Descripcion,");
                    sb.AppendLine("m.IdMarca, m.Descripcion[DesMarca],");
                    sb.AppendLine("c.idCategoria,c.Descripcion[DesCategoria],");
                    sb.AppendLine("p.Precio,p.Stock,p.RutaImagen,p.NombreImagen,p.Activo");
                    sb.AppendLine("from PRODUCTO p");
                    sb.AppendLine("inner join MARCA m on m.IdMarca = p.IdMarca");
                    sb.AppendLine("inner join Categoria c on c.idCategoria = p.IdCategoria");

                   


                    // hago un comando SQL con la conexión y el query
                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
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
                            lista.Add(new Producto() // Aquí creo un nuevo objeto Usuario y lo lleno con los datos del reader
                            {
                                // Asigno los valores del reader a las propiedades del objeto Usuario
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion= reader["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                oMarca = new Marca() { IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                    Descripcion = reader["DesMarca"].ToString(),
                                },
                                oCategoria = new Categoria() { idCategoria = Convert.ToInt32(reader["idCategoria"]),
                                    Descripcion = reader["DesCategoria"].ToString(),
                                },
                                Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-CR")),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                RutaImagen = reader["RutaImagen"].ToString(),
                                NombreImagen = reader["NombreImagen"].ToString()
                            });

                        }
                    }
                }
            }
            // Si ocurre un error, lo capturo y no hago nada, solo retorno la lista vacía
            catch
            {
                lista = new List<Producto>();

            }
            return lista;
        }// fin del metodo Listar


        public int Registrar(Producto obj, out string Mensaje)
        {
            int idGenerado = 0; // Variable para almacenar el ID generado
            Mensaje = string.Empty; // Inicializo el mensaje como vacío

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarProducto", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarUsuario'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el usuario
                    comando.Parameters.AddWithValue("Nombre", obj.Nombre);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Descripcion", obj.Descripcion);// Apellidos del usuario, que se almacenará en la base de dato
                    comando.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("IdCategoria", obj.oCategoria.idCategoria);
                    comando.Parameters.AddWithValue("Precio", obj.Precio);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Stock", obj.Stock);
                    comando.Parameters.AddWithValue("Activo", obj.Activo);// Nombres del usuario, que se almacenará en la base de datos
                 
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


        public bool Editar(Producto obj, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la edición fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_EditarProducto", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_EditarUsuario'
                    comando.Parameters.AddWithValue("IdProducto", obj.IdProducto);// ID de la categoria
                    comando.Parameters.AddWithValue("Nombre", obj.Nombre);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Descripcion", obj.Descripcion);// Apellidos del usuario, que se almacenará en la base de dato
                    comando.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("IdCategoria", obj.oCategoria.idCategoria);
                    comando.Parameters.AddWithValue("Precio", obj.Precio);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("Stock", obj.Stock);
                    comando.Parameters.AddWithValue("Activo", obj.Activo);// 
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




        // metodo nuevo para tener el control de rutaImagen y NombreImagen

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {

            bool resultado = false; // Variable para indicar si la operación fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {

                    string query = "update producto set RutaImagen =@rutaimagen,NombreImagen = @nombreimagen where IdProducto = @idproducto";


                    SqlCommand comando = new SqlCommand(query, oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_RegistrarUsuario'
                    // Aquí llamo al procedimiento almacenado que se encargará de registrar el usuario
                    comando.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);// Nombres del usuario, que se almacenará en la base de datos
                    comando.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);// Apellidos del usuario, que se almacenará en la base de dato
                    comando.Parameters.AddWithValue("@idproducto", obj.IdProducto);
                    comando.CommandType = CommandType.Text;// Indico que es un procedimiento almacenado

                    oconexion.Open(); // Abro la conexión a la base de datos

                    if (comando.ExecuteNonQuery() > 0)
                    {
                        resultado = true; // Si la ejecución del comando afecta a alguna fila, indico que la operación fue exitosa
                    }
                    else
                    {
                        // Si no se afectó ninguna fila, indico que la operación falló
                        Mensaje = "No se pudo guardar los datos de la imagen."; // Asigno un mensaje de error // Ejecuto el comando


                    }

                }
            }

            catch (Exception ex)
            {
                // Si ocurre un error, capturo la excepción y asigno el mensaje de error
                resultado = false; // Si hay un error, el ID generado será 0
                Mensaje = ex.Message;// Asigno el mensaje de error a la variable Mensaje
            }

            return resultado; // Retorno true si la operación fue exitosa, false si hubo un error


        }























        public bool Eliminar(int id, out string Mensaje)
        {
            bool respuesta = false; // Variable para indicar si la edición fue exitosa
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) // Uso la conexión definida en la clase Conection
                {
                    SqlCommand comando = new SqlCommand("sp_EliminarProducto", oconexion); // Creo un comando SQL para llamar al procedimiento almacenado 'sp_EditarUsuario'
                    comando.Parameters.AddWithValue("IdProducto", id);// ID de la Producto a eliminar
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
                Mensaje =" El producto en encuentra relacionado a una venta, un usuario lo tiene en su carrito  ";// Asigno el mensaje de error a la variable Mensaje
            }
            return respuesta; // Retorno true si la edición fue exitosa, false si hubo un error
        }// fin del método



        public List<Producto> ObtenerProductos(int idMarca, int idCategoria, int nroPagina, int obtenerRegistros, out int TotalRegistros, out int TotalPaginas)
        {
            List<Producto> lista = new List<Producto>();
            TotalRegistros = 0;
            TotalPaginas = 0;

            try
            {
               
                using (SqlConnection oconexion = new SqlConnection(Conection.cn)) 

                {

                   
                    SqlCommand cmd = new SqlCommand("sp_ObtenerProductos", oconexion);
                    cmd.Parameters.AddWithValue("idMarca", idMarca);
                    cmd.Parameters.AddWithValue("idCategoria", idCategoria);// Nombres del usuario, que se almacenará en la base de datos
                    cmd.Parameters.AddWithValue("nroPagina", nroPagina);
                    cmd.Parameters.AddWithValue("obtenerRegistros", obtenerRegistros);// Nombres del usuario, que se almacenará en la base de datos

                    cmd.Parameters.Add("TotalRegistros", SqlDbType.Int).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un entero
                    cmd.Parameters.Add("TotalPaginas", SqlDbType.Int).Direction = ParameterDirection.Output;// Indico que este parámetro es de salida y será un string de hasta 500 caracteres
                    cmd.CommandType = CommandType.StoredProcedure;// Indico que es un procedimiento almacenado

                 

                    oconexion.Open();
                
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            lista.Add(new Producto() 
                            {
                            
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                oMarca = new Marca()
                                {
                                    IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                    Descripcion = reader["DesMarca"].ToString(),
                                },
                                oCategoria = new Categoria()
                                {
                                    idCategoria = Convert.ToInt32(reader["idCategoria"]),
                                    Descripcion = reader["DesCategoria"].ToString(),
                                },
                                Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-CR")),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                RutaImagen = reader["RutaImagen"].ToString(),
                                NombreImagen = reader["NombreImagen"].ToString()
                            });

                        }
                    }
                    TotalRegistros = Convert.ToInt32(cmd.Parameters["TotalRegistros"].Value);
                    TotalPaginas = Convert.ToInt32( cmd.Parameters["TotalPaginas"].Value); 
                }
            }
           
            catch
            {
                lista = new List<Producto>();

            }
            return lista;
        }// fin del metodo Listar

    }
}
