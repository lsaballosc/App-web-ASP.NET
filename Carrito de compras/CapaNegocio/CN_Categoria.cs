using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Categoria
    {

        private CD_Categoria objCategoria = new CD_Categoria();




        // metodo para listar las categorias

        public List<Categoria> Listar()
        {
            //llamo al metodo de la capa de datos que me devuelve una lista de usuarios


            return objCategoria.Listar();

        }
        // fin del metodo listar



        public int Registrar(Categoria obj, out string Mensaje)
        {
            //llamo al metodo de la capa de datos que me devuelve el id generado y un mensaje de error si ocurre

            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripción de la categoría no puede ser vacía.";

            }
           
            //si el mensaje esta vacio, significa que no hay errores en los campos obligatorios
            if (string.IsNullOrEmpty(Mensaje))
            {

             
               
                return objCategoria.Registrar(obj, out Mensaje); // Llamo al método de la capa de datos para registrar la categoría y obtengo el mensaje de error si lo hay


            }
            else
            {
                return 0;
            }// Retorno 0 si hay un mensaje de error, indicando que no se pudo registrar

        }
        // fin


        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "la Descripción no puede estar vacía";
                return false;
            }
           
            //si el mensaje esta vacio, significa que no hay errores en los campos obligatorios
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCategoria.Editar(obj, out Mensaje); // Llamo al método de la capa de datos para editar el usuario y obtengo el mensaje de error si ocurre
            }
            else
            {
                return false; // Retorno false si hay un mensaje de error, indicando que no se pudo editar
            }
        }

        public bool Eliminar(int idCategoria, out string Mensaje)
        {
            return objCategoria.Eliminar(idCategoria, out Mensaje); // Llamo al método de la capa de datos para eliminar el usuario y obtengo el mensaje de error si ocurre
        }


    }
}