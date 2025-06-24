using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {

        private CD_Productos objProducto = new CD_Productos();




        // metodo para listar las categorias

        public List<Producto> Listar()
        {
            //llamo al metodo de la capa de datos que me devuelve una lista de usuarios


            return objProducto.Listar();

        }
        // fin del metodo listar



        public int Registrar(Producto obj, out string Mensaje)
        {
            //llamo al metodo de la capa de datos que me devuelve el id generado y un mensaje de error si ocurre

            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Producto no puede ser vacía.";

            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripción del Producto no puede ser vacía.";

            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca válida.";
            }
            else if (obj.oCategoria.idCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoría válida.";
            }
            else if(obj.Precio == 0){ 
                Mensaje = "Debe Ingresar el precio del producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }
            //si el mensaje esta vacio, significa que no hay errores en los campos obligatorios
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objProducto.Registrar(obj, out Mensaje); // Llamo al método de la capa de datos para registrar la categoría y obtengo el mensaje de error si lo hay
            }
            else
            {
                return 0;
            }// Retorno 0 si hay un mensaje de error, indicando que no se pudo registrar

        }
        // fin


        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Producto no puede ser vacía.";

            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripción del Producto no puede ser vacía.";

            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca válida.";
            }
            else if (obj.oCategoria.idCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoría válida.";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Debe Ingresar el precio del producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }

            //si el mensaje esta vacio, significa que no hay errores en los campos obligatorios
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objProducto.Editar(obj, out Mensaje); // Llamo al método de la capa de datos para editar el usuario y obtengo el mensaje de error si ocurre
            }
            else
            {
                return false; // Retorno false si hay un mensaje de error, indicando que no se pudo editar
            }
        }



        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return objProducto.GuardarDatosImagen(obj, out Mensaje); // Llamo al método de la capa de datos para guardar los datos de la imagen y obtengo el mensaje de error si ocurre
        }
        public bool Eliminar(int idCategoria, out string Mensaje)
        {
            return objProducto.Eliminar(idCategoria, out Mensaje); // Llamo al método de la capa de datos para eliminar el usuario y obtengo el mensaje de error si ocurre
        }


    }








}

