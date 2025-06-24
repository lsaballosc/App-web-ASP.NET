using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AppWeb.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor

        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }
        // Categorías
        #region CATEGORIAS
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            // Creo un objeto de la capa de Entidad para acceder a los metodos de la capa de negocio
            List<Categoria> olista = new List<Categoria>();
            olista = new CN_Categoria().Listar();

            // Retorno un JsonResult con la lista de usuarios
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]

        public JsonResult GuardarCategoria(Categoria obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.idCategoria == 0)
            {
                // Si el IdCategoria es 0, significa que es un nuevo usuario
                resultado = new CN_Categoria().Registrar(obj, out mensaje);
            }
            else
            {
                // Si el IdUsuario es mayor a 0, significa que es un usuario existente
                resultado = new CN_Categoria().Editar(obj, out mensaje);
            }

            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            // hago 2 variables para el resultado y el mensaje
            string mensaje = string.Empty;

            bool respuesta = false;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);
            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        // Marcas

        #region MARCAS
        [HttpGet]
        public JsonResult ListarMarca()
        {
            // Creo un objeto de la capa de Entidad para acceder a los metodos de la capa de negocio
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marca().Listar();

            // Retorno un JsonResult con la lista de usuarios
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]

        public JsonResult GuardarMarca(Marca obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdMarca == 0)
            {
                // Si el IdCategoria es 0, significa que es un nuevo usuario
                resultado = new CN_Marca().Registrar(obj, out mensaje);
            }
            else
            {
                // Si el IdUsuario es mayor a 0, significa que es un usuario existente
                resultado = new CN_Marca().Editar(obj, out mensaje);
            }

            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            // hago 2 variables para el resultado y el mensaje
            string mensaje = string.Empty;

            bool respuesta = false;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);
            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PRODUCTOS

        [HttpGet]
        public JsonResult ListarProducto()
        {
            // Creo un objeto de la capa de Entidad para acceder a los metodos de la capa de negocio
            List<Producto> olista = new List<Producto>();
            olista = new CN_Producto().Listar();

            // Retorno un JsonResult con la lista de usuarios
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        // Este metodo recibe un objeto Producto en formato string y un archivo de imagen
        public JsonResult GuardarProducto(string obj, HttpPostedFileBase archivoImagen)
        {
           // object resultado;
            string mensaje = string.Empty;
            bool operacion_Exitosa = true;
            bool guardarImagenExitosa = true;
            Producto oProducto = new Producto();
            //recibo el objeto en formato string y lo convierto a un objeto Producto
            oProducto = JsonConvert.DeserializeObject<Producto>(obj);

            decimal precio;
            // le digo que el precio es un decimal y que lo voy a parsear desde el texto del objeto Producto, adempas de que voy a usar la cultura de Costa Rica para el formato decimal
            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("es-CR"), out precio))
            {
                // Si el parseo es exitoso, asigno el valor al objeto Producto
                oProducto.Precio = precio;
            }
            else
            {
               // operacion_Exitosa = false;
                //mensaje = "No se ha podido ingresar debido a que el precio no está en el formato correcto";
               return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.## o ####.##,#" }, JsonRequestBehavior.AllowGet);
            }
            if (oProducto.IdProducto == 0)
            {
                // Si el IdCategoria es 0, significa que es un nuevo usuario
                int idProuctoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProuctoGenerado != 0)
                {
                    oProducto.IdProducto = idProuctoGenerado; // Asigno el Id generado al objeto Producto
                }
                else
                {
                    operacion_Exitosa = false; // Si no se pudo registrar el producto, cambio la variable de operacion exitosa a false
                }
            }
            else
            {
                // el resultado es si la operacion fue exitosa o no
                operacion_Exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }


            //logica para guardar la imagen
            // si todo salió bien, guardo la imagen
            if (operacion_Exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    // el nombre será el id del producto + la extension del archivo
                    string nomeArchivo = string.Concat(oProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nomeArchivo));

                    }
                    catch (Exception ex)
                    {
                        // Si ocurre un error al guardar la imagen, cambio la variable de operacion exitosa a false
                        operacion_Exitosa = false;
                        mensaje = "Error al guardar la imagen: " + ex.Message;
                    }


                    if (guardarImagenExitosa)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nomeArchivo;

                        bool resp = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "No se pudo guardar la imagen del producto.";
                        operacion_Exitosa = false;
                    }
                }

            }
            return Json(new { operacionExitosa = operacion_Exitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //

        //metodo que va a devolver una cadena en base 64 de la imagen del producto
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            // hago una variable de tipo bool para saber si la conversion fue exitosa o no
            bool conversion;
            // creo un objeto de la capa de negocio para acceder a los metodos de la capa de negocio y obtener el producto por su id
            Producto oProducto = new CN_Producto().Listar().Where(p=> p.IdProducto == id).FirstOrDefault();
            // si el producto es nulo, retorno un mensaje de error, sino, convierto la imagen a base 64, como la ruta y el nombre de la imagen del producto
            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen),out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            },JsonRequestBehavior.AllowGet );
        }

        public JsonResult EliminarProducto(int id)
        {
            // hago 2 variables para el resultado y el mensaje
            string mensaje = string.Empty;

            bool respuesta = false;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);
            // Retorno un JsonResult con el resultado de la operacion
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}

