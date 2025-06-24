using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objMarca = new CD_Marca();




        // metodo para listar las categorias

        public List<Marca> Listar()
        {
            //llamo al metodo de la capa de datos que me devuelve una lista de usuarios


            return objMarca.Listar();

        }
        // fin del metodo listar



        public int Registrar(Marca obj, out string Mensaje)
        {
            //llamo al metodo de la capa de datos que me devuelve el id generado y un mensaje de error si ocurre

            Mensaje = string.Empty; // Inicializo el mensaje como vacío
            // Validar campos obligatorios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripción de la Marca no puede ser vacía.";

            }

            //si el mensaje esta vacio, significa que no hay errores en los campos obligatorios
            if (string.IsNullOrEmpty(Mensaje))
            {



                return objMarca.Registrar(obj, out Mensaje); // Llamo al método de la capa de datos para registrar la categoría y obtengo el mensaje de error si lo hay


            }
            else
            {
                return 0;
            }// Retorno 0 si hay un mensaje de error, indicando que no se pudo registrar

        }
        // fin


        public bool Editar(Marca obj, out string Mensaje)
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
                return objMarca.Editar(obj, out Mensaje); // Llamo al método de la capa de datos para editar el usuario y obtengo el mensaje de error si ocurre
            }
            else
            {
                return false; // Retorno false si hay un mensaje de error, indicando que no se pudo editar
            }
        }

        public bool Eliminar(int idCategoria, out string Mensaje)
        {
            return objMarca.Eliminar(idCategoria, out Mensaje); // Llamo al método de la capa de datos para eliminar el usuario y obtengo el mensaje de error si ocurre
        }

        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            return objMarca.ListarMarcaporCategoria(idcategoria);
        }

    }
}

